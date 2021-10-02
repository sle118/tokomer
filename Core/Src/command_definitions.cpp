#include "command_definitions.h"
#include "cmsis_os.h"

#include "usbd_cdc_if.h"
#include "main.h"
#include "debug_helper.h"
#include "display_thread.h"

#define QUEUE_LEN 5
static command_fifo_t queue = { 0 };
//static actions_t *writePtr = &queue[0];
//static actions_t *readptr = &queue[0];
static char outBuffer[25] = { 0 };
static const char *commands_string[] = { "NOOP", "reset", "graphvolt",
		"graphcurr", "serialon", "serialoff", "poweron", "poweroff", "scale0",
		"scale1", "scale2", "scale3", "digitalon", "digitaloff", "status","binaryon","binaryoff","help",
		NULL };

/* Create FIFO*/
FIFO RX_FIFO = { .head = 0, .tail = 0 };

// poor man's locking mechanism - code could still run into concurrency issues
static volatile bool lock = false;

void initialize_commands() {

}
void enqueue(actions_t cmd) {
	//osMessagePut(MsgBox, (uint32_t) cmd, osWaitForever);  // Send Message
	if (cmd == ACTION_NONE)
		return;
	while (lock) {
		osThreadYield();
	}
	lock = true;
	FIFO_PUSH(queue, cmd);
	if (FIFO_OVERRUN(queue)) {
		// well... commands are not processed quick enough!
	}
	lock = false;
}
void show_help() {
	uint8_t cmd_index = 1; // help is command 0, so skip it
	const char *menu_header = "Valid commands: \r\n";
	CDC_Transmit_FS((uint8_t*) menu_header, strlen(menu_header));
	do {
		strncpy(outBuffer, commands_string[cmd_index], sizeof(outBuffer) - 1);
		strncat(outBuffer, "\r\n", sizeof(outBuffer) - 1);
		uint8_t res = USBD_OK;
		do {
			res = CDC_Transmit_FS((uint8_t*) outBuffer, strlen(outBuffer));
		} while (res == USBD_BUSY);

		cmd_index++;
	} while (commands_string[cmd_index] != NULL);
}
command_result_t handle_command_string(uint8_t *cmd_string) {
	uint8_t cmd_index = 0;
	command_result_t res = COMMAND_RESULT_UNKNOWN;
	do {
		if (strcmp((const char*) cmd_string, commands_string[cmd_index]) == 0) {
			enqueue((actions_t) cmd_index);
			res = COMMAND_RESULT_OK;
			break;
		}
		cmd_index++;
	} while (commands_string[cmd_index] != NULL);
	return res;
}

bool dequeue(actions_t *cmd) {

	if (cmd) {
		*cmd = ACTION_NONE;
	} else {
		return !(FIFO_DATA_END(queue));
	}
	if (FIFO_DATA_END(queue))
		return false;
	while (lock) {
		osThreadYield();
	}
	lock = true;
	FIFO_POP_VALUE(*cmd, queue);
	lock = false;
	return true;
}

int8_t calibrationStep = 0;

#define MAX_CALIBRATION_STEPS 9

void reset_stats() {
	totalBusMicroAmps = 0;
	lnow = 0;
	didx = 127;
	for (int i = 0; i < 128; i++) {
		plot[i] = 0;
		plot1[i] = 0;
		plot2[i] = 0;
		plot3[i] = 0;
		plot4[i] = 0;
		plot5[i] = 0;
	}
}
void calibrate() {
	// calibration
	if (calibrationStep > 0) {
		switch (calibrationStep) {
		case 9:      // set 3 rd range
			forcedRange = 0;
			voltageK = 17000;
			ina226 = 1;
			HAL_GPIO_WritePin(ONEKLOAD_GPIO_Port, ONEKLOAD_Pin, GPIO_PIN_SET);
			zero = lsumBusMicroAmpsOrig / lreadings;
			_DEBUG("Zero I %d\n", zero);
			break;
		case 7:
			forcedRange = 3;
			ranges[3] = 10700;
			// calibration current in ua on 1K is voltage in mv
			voltageK = (lsumBusMillVoltsOrig * voltageK) / lsumBusMillVolts;
			_DEBUG("INA226 %u mV, STM32 %u mV, INA226 %u uA\n",
					(uint) lsumBusMillVoltsOrig / lreadings,
					(uint) lsumBusMillVolts / lreadings,
					(uint) lsumBusMicroAmps / lreadings);
			_DEBUG("Voltage K %u\n", voltageK);
			break;
		case 5:
			forcedRange = 2;
			ranges[2] = 1222;
			ranges[3] = (lsumBusMillVoltsOrig * ranges[3])
					/ lsumBusMicroAmpsOrig;
			_DEBUG("INA226 %u mV, STM32 %u mV, INA226 %u uA\n",
					(uint) lsumBusMillVoltsOrig / lreadings,
					(uint) lsumBusMillVolts / lreadings,
					(uint) lsumBusMicroAmps / lreadings);
			_DEBUG("Range 3 K %u\n", ranges[3]);
			break;
		case 3:
			forcedRange = 0;
			global.power = 1;
			ina226 = 0;
			HAL_GPIO_WritePin(ONEKLOAD_GPIO_Port, ONEKLOAD_Pin, GPIO_PIN_RESET);
			ranges[2] = (lsumBusMillVoltsOrig * ranges[2]) / lsumBusMicroAmps;
			_DEBUG("INA226 %u mV, STM32 %u mV, INA226 %u uA\n",
					(uint) lsumBusMillVoltsOrig / lreadings,
					(uint) lsumBusMillVolts / lreadings,
					(uint) lsumBusMicroAmps / lreadings);
			_DEBUG("Range 2 K %u\n", ranges[2]);
			break;
		case 1:
			reset_stats();
			calibrationStep = 0;
			break;
		}
		calibrationStep--;
	}
}
static const char* bufferedNumber(int value) {
	static char buffer[13] = { 0 };
	return itoa(value, buffer, sizeof(buffer));
}
bool prev_enable ;
bool prev_binaryenable ;
void holdDataTransfer()
{
	prev_enable=global.serialEnable;
	prev_binaryenable=global.serialBinaryEnable;
	global.serialEnable=false;
	global.serialBinaryEnable=false;
}
void resumeDataTransfer()
{
	global.serialEnable=prev_enable;
	global.serialBinaryEnable=prev_binaryenable;
}
void strncat_flush(char *buffer, const char *value, size_t bufSize) {
	uint8_t res = USBD_OK;
	size_t outlen = strlen(buffer);
	if (!value || (strlen(value) + outlen >= bufSize)) {
		// disable data transfer to avoid colliding

		do {

			res = CDC_Transmit_FS((uint8_t*) buffer, outlen);
		} while (res == USBD_BUSY);
		memset(buffer, 0x00, bufSize);

	}
	if (value) {
		strncat(buffer, value, bufSize);
	}
}
static const char *curly_open = "{";
static const char *curly_close = "}";
static const char *square_open = "[";
static const char *square_close = "]";
static const char *comma = ",";
static const char *quote = "\"";
static const char *semicolon = ":";


#define ADD_QUOTED_STRING(buffer,value) strncat_flush(buffer,quote,sizeof(buffer)-1); strncat_flush(buffer,value,sizeof(buffer)-1); strncat_flush(buffer,quote,sizeof(buffer)-1);
#define ADD_VARIABLE_ENTRY(buffer,value) ADD_QUOTED_STRING(buffer,value); strncat_flush(buffer,semicolon,sizeof(buffer)-1)
#define ADD_BOOL_VALUE(buffer,value) strncat_flush(buffer,(value?"true":"false"),sizeof(buffer)-1);
#define ADD_BOOL_VARIABLE(buffer,name,value, last) ADD_VARIABLE_ENTRY(buffer,name); ADD_BOOL_VALUE(buffer,value); if(!last) strncat_flush(buffer,comma,sizeof(buffer)-1);
#define ADD_STRING_VARIABLE(buffer,name,value, last) ADD_VARIABLE_ENTRY(buffer,name); ADD_QUOTED_STRING(buffer,value) if(!last) strncat_flush(buffer,comma,sizeof(buffer)-1);
#define ADD_INTEGER_VARIABLE(buffer,name,value, last) ADD_VARIABLE_ENTRY(buffer,name); strncat_flush(buffer,bufferedNumber(value),sizeof(buffer)-1);   if(!last) strncat_flush(buffer,comma,sizeof(buffer)-1);


//"graphvolt", "graphcurr", "serialon", "serialoff", "poweron", "poweroff", "sclale0",
//		"sclale1", "sclale2", "sclale3", "digitalon", "digitaloff", "status","help",
//"binaryon","binaryoff"

void reportStatus() {
	memset(outBuffer, 0x00, sizeof(outBuffer));
	holdDataTransfer();
	strncat_flush(outBuffer, curly_open, sizeof(outBuffer) - 1);
	ADD_STRING_VARIABLE(outBuffer, "graph", global.gmode>0?"volt":"curr", false);
	ADD_BOOL_VARIABLE(outBuffer, "serial", global.serialEnable, false);
	ADD_BOOL_VARIABLE(outBuffer, "power", (global.power > 0), false);
	ADD_INTEGER_VARIABLE(outBuffer, "scale", global.rangeScale, false);
	ADD_BOOL_VARIABLE(outBuffer, "digital", (global.digitalInputEnable > 0),false);
	ADD_BOOL_VARIABLE(outBuffer, "binary", global.serialBinaryEnable, false);
	ADD_BOOL_VARIABLE(outBuffer, "overload", global.overload, false);
	ADD_VARIABLE_ENTRY(outBuffer, "commands");
	strncat_flush(outBuffer, square_open, sizeof(outBuffer) - 1);
	uint8_t cmd_index = 0;
	do {
		if (cmd_index > 0) {
			strncat_flush(outBuffer, comma, sizeof(outBuffer) - 1);
		}
		ADD_QUOTED_STRING(outBuffer, commands_string[cmd_index]);
		cmd_index++;
	} while (commands_string[cmd_index] != NULL);
	strncat_flush(outBuffer, square_close, sizeof(outBuffer) - 1);
	strncat_flush(outBuffer, curly_close, sizeof(outBuffer) - 1);
	strncat_flush(outBuffer, "\n", sizeof(outBuffer) - 1);
	strncat_flush(outBuffer, NULL, sizeof(outBuffer) - 1); // flush the output buffer
	resumeDataTransfer();
}
void handleCommandQueue() {
	actions_t cmd;
	if (!dequeue(&cmd)) {
		return;
	}
	switch (cmd) {
	case ACTION_NONE:
		return;
	case ACTION_HELP:
		show_help();
		return;
	case ACTION_GET_STATUS:
		reportStatus();
		break;
	case ACTION_RESET_STATS:
		reset_stats();
		break;
	case ACTION_GRAPH_SELECT_VOLTAGE:
		global.gmode = 0;
		break;
	case ACTION_GRAPH_SELECT_CURRENT:
		global.gmode = 1;
		break;
	case ACTION_SERIAL_ON:
		global.serialBinaryEnable = false;
		global.serialEnable = true;
		break;
	case ACTION_SERIAL_OFF:
		global.serialEnable = false;
		break;
	case ACTION_POWER_ON:
		calibrationStep = MAX_CALIBRATION_STEPS;
		global.overload = 0;
		break;
	case ACTION_POWER_OFF:
		global.power = 0;
		global.overload = 0;
		break;
	case ACTION_SET_RANGE_SCALE_0:
		global.rangeScale = 0;
		break;
	case ACTION_SET_RANGE_SCALE_1:
		global.rangeScale = 1;
		break;
	case ACTION_SET_RANGE_SCALE_2:
		global.rangeScale = 2;
		break;
	case ACTION_SET_RANGE_SCALE_3:
		global.rangeScale = 3;
		break;
	case ACTION_DIGITALIN_ON:
		requestedDigitalInputEnable = true;
		break;
	case ACTION_DIGITALIN_OFF:
		requestedDigitalInputEnable = false;
		break;
	case ACTION_BINARY_ON:
		global.serialEnable=false;
		global.serialBinaryEnable = true;
		break;
	case ACTION_BINARY_OFF:
		global.serialBinaryEnable = false;
		break;
	default:
		break;
	}
}
void process() {
	handleCommandQueue();
	calibrate();
}
//
// Small code snip taken here
// https://hackaday.io/project/20879-notes-on-using-systemworkbench-with-stm32-bluepill/log/57048-hints-for-using-the-cdc-usb-serial
//
uint8_t VCP_read_line(uint8_t *Buf, uint32_t Len) {
	uint32_t count = 0;
	/* Check inputs */
	if ((Buf == NULL) || (Len == 0)) {
		return 0;
	}

	while (Len--) {
		if (FIFO_DATA_END(RX_FIFO))
			return count;
		if (RX_FIFO.data[RX_FIFO.tail] == '\r'
				|| RX_FIFO.data[RX_FIFO.tail] == '\n') {
			*Buf = '\0';
			FIFO_INCR_TAIL(RX_FIFO);
			break;
		}
		count++;
		*Buf++ = RX_FIFO.data[RX_FIFO.tail];
		FIFO_INCR_TAIL(RX_FIFO);
	}
	if (Len > 0 && *Buf != '\0') {
		*Buf = '\0';
	} else if (Len == 0) {
		*--Buf = '\0';
		count--;
	}
	return count;
}
static system_state_t prevstate = { 0 };
void StateChangeCheck() {
	if (memcmp(&prevstate, (const void *)&global, sizeof(prevstate)) != 0) {
		enqueue(ACTION_GET_STATUS);
		memcpy(&prevstate, (const void *)&global, sizeof(prevstate));
	}

}
void USBRx(void const *arg) {
	osHandlehandleUSBDataRXId = osThreadGetId();
	uint8_t CommandBuffer[15] = { 0 };
	for (;;) {
		osEvent event = osSignalWait(SIGNAL_CR_RECEIVED | SIGNAL_DATA_RECEIVED,
				125);
		if (event.status == osEventSignal) {

			if (event.value.signals & SIGNAL_CR_RECEIVED) {
				uint8_t len = VCP_read_line(CommandBuffer,
						sizeof(CommandBuffer));
				if (len > 0) {
					if (handle_command_string(CommandBuffer)
							== COMMAND_RESULT_UNKNOWN) {
						//show_help();
					}
				}
			} else if (event.value.signals & SIGNAL_DATA_RECEIVED) {
				if (FIFO_OVERRUN(RX_FIFO)) {
					// flush a bit of the buffer. This should not happen
					VCP_read_line(CommandBuffer, sizeof(CommandBuffer));
				}
			}

		}
		StateChangeCheck();
	}

}

// returns bytes read (could be zero)
// would be easy to make it end early on a stop char (e.g., \r or \n)
uint8_t VCP_read(uint8_t *Buf, uint32_t Len) {
	uint32_t count = 0;
	/* Check inputs */
	if ((Buf == NULL) || (Len == 0)) {
		return 0;
	}

	while (Len--) {
		if (RX_FIFO.head == RX_FIFO.tail)
			return count;
		count++;
		*Buf++ = RX_FIFO.data[RX_FIFO.tail];
		FIFO_INCR_TAIL(RX_FIFO);
	}

	return count;
}
