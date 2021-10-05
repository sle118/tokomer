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
	if(cmd==ACTION_GET_STATUS)
	{
		statusRequested = true;
	}
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
			enqueue(ACTION_GET_STATUS); //  we're enabling power here
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

void strncat_flush(const char *value) {
	uint8_t res = USBD_OK;
	size_t outlen = strlen(outBuffer);
	if (!value || (strlen(value) + outlen >= sizeof(outBuffer)-1)) {
		do {

			res = CDC_Transmit_FS((uint8_t*) outBuffer, outlen);
		} while (res == USBD_BUSY);
		memset(outBuffer, 0x00, sizeof(outBuffer));

	}
	if (value) {
		strncat(outBuffer, value, sizeof(outBuffer)-1);
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
#define ADD_BOOL_VALUE(buffer,value) ,sizeof(buffer)-1);
#define ADD_BOOL_VARIABLE(buffer,name,value, last) ADD_VARIABLE_ENTRY(buffer,name); ADD_BOOL_VALUE(buffer,value); if(!last) strncat_flush(buffer,comma,sizeof(buffer)-1);
#define ADD_STRING_VARIABLE(buffer,name,value, last) ADD_VARIABLE_ENTRY(buffer,name); ADD_QUOTED_STRING(buffer,value) if(!last) strncat_flush(buffer,comma,sizeof(buffer)-1);
#define ADD_INTEGER_VARIABLE(buffer,name,value, last) ADD_VARIABLE_ENTRY(buffer,name);    if(!last) strncat_flush(buffer,comma,sizeof(buffer)-1);


//"graphvolt", "graphcurr", "serialon", "serialoff", "poweron", "poweroff", "sclale0",
//		"sclale1", "sclale2", "sclale3", "digitalon", "digitaloff", "status","help",
//"binaryon","binaryoff"
void addVar(const char * name)
{
	strncat_flush(quote);
	strncat_flush(name);
	strncat_flush(quote);
	strncat_flush(semicolon);
}
void addStringVar(const char * name, const char * value, bool last)
{
	addVar(name);
	strncat_flush(quote);
	strncat_flush(value);
	strncat_flush(quote);
	if(!last) strncat_flush(comma);
}

void addBoolVar(const char * name, bool value, bool last)
{
	addVar(name);
	strncat_flush(value?"true":"false");
	if(!last) strncat_flush(comma);
}
void addIntVar(const char * name, int value, bool last)
{
	addVar(name);
	strncat_flush(bufferedNumber(value));
	if(!last) strncat_flush(comma);
}
void reportStatus() {
	memset(outBuffer, 0x00, sizeof(outBuffer));
	dataTransferHold=true;
	strncat_flush(curly_open);
	addStringVar("graph", !global.graphModeCurrent?"volt":"curr", false);
	addBoolVar("serial", global.serialEnable, false);
	addBoolVar("power", global.power , false);
	addIntVar("scale", global.rangeScale, false);
	addBoolVar("digital", global.digitalInputEnable,false);
	addBoolVar("binary", global.serialBinaryEnable, false);
	addBoolVar("overload", global.overload, false);
	addVar("commands");
	strncat_flush(square_open);
	uint8_t cmd_index = 0;
	do {
		if (cmd_index > 0) {
			strncat_flush(comma);
		}
		strncat_flush(quote);
		strncat_flush(commands_string[cmd_index]);
		strncat_flush(quote);
		cmd_index++;
	} while (commands_string[cmd_index] != NULL);
	strncat_flush(square_close);
	strncat_flush(curly_close);
	strncat_flush("\n");
	strncat_flush(NULL); // flush the output buffer
	dataTransferHold=false;
	statusRequested = false;
}
void handleCommandQueue() {
	actions_t cmd;
	bool reportChange=true;
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
		return;
		break;
	case ACTION_RESET_STATS:
		reset_stats();
		break;
	case ACTION_GRAPH_SELECT_VOLTAGE:
		global.graphModeCurrent = false;
		break;
	case ACTION_GRAPH_SELECT_CURRENT:
		global.graphModeCurrent = true;
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
	if(reportChange) {
		enqueue(ACTION_GET_STATUS);
	}
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
