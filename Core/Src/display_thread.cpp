#include "main.h"
#include "command_definitions.h"
#include "cmsis_os.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include "debug_helper.h"
#include "ssd1306.h"
#include "standard_font.h"

SSD1306 oled;

int32_t plot[128], plot1[128], plot2[128], plot3[128], plot4[128], plot5[128],
		pmax, pmin;
uint32_t scale;
uint8_t pidx = 0, didx = 128;

enum button {
	NOKEY, KEY1, KEY2, KEY3
};
uint16_t buttonTime;
uint8_t buttonCode;

char sbuf[32];
void printFloat(int x, int y,float v, int8_t maxDigits, bool enlargeForMinus,
		const char *suffix) {
	oled.setCursor(x, y);
	float ov = v;
	v = abs(v);
	itoa((int) v, sbuf, 10);
	int16_t i = maxDigits - strlen(sbuf);
	if (ov < 0) {
		oled.putc('-');
		i -= enlargeForMinus ? 0 : 1;
	}
	oled.puts(sbuf);
	if (i > 0) {
		i = i == 1 ? 10 : (i == 2 ? 100 : 1000);
		oled.putc('.');
		itoa(abs((int) (i * v)) % i + i, sbuf, 10);
		oled.puts(sbuf + 1);
	}
	oled.puts(suffix);
}

extern "C" uint16_t EE_WriteVariable(uint16_t VirtAddress, uint16_t Data);

extern "C" void updateScreenX(void const *arg);

void handleButtons() {
	uint8_t lbuttonCode = NOKEY;
	if (HAL_GPIO_ReadPin(KEY1_GPIO_Port, KEY1_Pin) == GPIO_PIN_RESET)
		lbuttonCode = KEY1;
	else if (HAL_GPIO_ReadPin(KEY2_GPIO_Port, KEY2_Pin) == GPIO_PIN_RESET)
		lbuttonCode = KEY2;
	else if (HAL_GPIO_ReadPin(KEY3_GPIO_Port, KEY3_Pin) == GPIO_PIN_RESET)
		lbuttonCode = KEY3;

	if (lbuttonCode == NOKEY && buttonCode != NOKEY) {
		if (buttonCode == KEY1 && buttonTime > 500) {
			enqueue(ACTION_RESET_STATS);
		} else if (buttonCode == KEY1) {
			// short KEY3 - toggle voltage o current graph
			if (!global.graphModeCurrent) {
				enqueue(ACTION_GRAPH_SELECT_CURRENT);
			} else {
				enqueue(ACTION_GRAPH_SELECT_VOLTAGE);
			}

		}

		if (buttonCode == KEY2) {
			if (buttonTime > 500) {
				// long KEY2, toogle sending data
				if (global.serial==SERIAL_OFF) {
					enqueue(ACTION_SERIAL_TEXT);
				} else {
					enqueue(ACTION_SERIAL_OFF);
				}
			} else {
				switch (global.rangeScale) {
				case 0:
					enqueue(ACTION_SET_RANGE_SCALE_1);
					break;
				case 1:
					enqueue(ACTION_SET_RANGE_SCALE_2);
					break;
				case 2:
					enqueue(ACTION_SET_RANGE_SCALE_3);
					break;
				default:
					enqueue(ACTION_SET_RANGE_SCALE_0);
					break;
				}
			}
		}
		if (buttonCode == KEY3) {
			// short KEY, toogle power
			if (global.power == 1)
				enqueue(ACTION_POWER_OFF);
			else
				enqueue(ACTION_POWER_ON);
		}
	}
	if (lbuttonCode != buttonCode) {
		buttonTime = 0;
		buttonCode = lbuttonCode;
	} else
		buttonTime += 100;
	return;
}
#define CHAR_FLIP_FLOP(x) (? x : ' ')
void updateScreenX(void const *arg) {
	osUpdateScreenThreadId = osThreadGetId();
	oled.initialise();
	oled.clear();
	oled.set_contrast(255);
	oled.set_font(bold_font, 8);

	float v;
	for (;;) {
		osSignalWait(0x1, 1000);
		// code here normall executes every ~100ms
		handleButtons();
		handleCommandQueue();
		calibrate();
		// voltage
		oled.clear();
		v = (float) (lsumBusMillVolts / lreadings) / 1000;
		printFloat(0,0,v, 3, false, "v");
		oled.setCursor(5, 0);
		oled.putc(global.digitalInputEnable?'D':' ');
		oled.setCursor(6, 0);
		if(global.power && (global.serial!=SERIAL_OFF)) {
			oled.putc(pidx % 2?' ':global.serial==SERIAL_TEXT?'^':'*');
		}
		else {
			oled.putc(' ');
		}
		// time

		itoa(lnow / 3600000, sbuf, 10);
		oled.setCursor(10 - strlen(sbuf), 0);
		oled.puts(sbuf);
		oled.putc(':');
		itoa(100 + (lnow / 60000) % 60, sbuf, 10);
		oled.puts(sbuf + 1);
		oled.putc(':');
		itoa(100 + (lnow / 1000) % 60, sbuf, 10);
		oled.puts(sbuf + 1);

		// graph
		if (global.overload == 0) {
			plot[pidx] = lmaxBusMicroAmps;
			plot1[pidx] = lminBusMicroAmps;
			plot2[pidx] = lsumBusMicroAmps / lreadings;
			plot3[pidx] = lmaxBusMillVolts;
			plot4[pidx] = lminBusMillVolts;
			plot5[pidx] = lsumBusMillVolts / lreadings;
		}

		int32_t *lpmin, *lpmax, *lpavg;
		if (global.graphModeCurrent ) {
			lpmin = plot1;
			lpmax = plot;
			lpavg = plot2;
		} else {
			lpmin = plot4;
			lpmax = plot3;
			lpavg = plot5;
		}

		pmin = lpmin[pidx], pmax = lpmax[pidx];

		uint8_t idx_start =
				didx > 0 ? ((pidx - (127 - didx)) % 128) : (pidx + 1) % 128;
		for (uint8_t i = 1; i < 128 - didx; i++) {
			uint8_t idx = (i + idx_start) % 128;
			if (lpmin[idx] < pmin)
				pmin = lpmin[idx];
			if (lpmax[idx] > pmax)
				pmax = lpmax[idx];
		}
		if (pmax == pmin)
			pmax += 1; // protect from device by zero
		if (global.power || global.overload ) {
			scale = ((1 << 25) * 36) / (pmax - pmin);
			uint8_t idx_start =
					didx > 0 ? ((pidx - (127 - didx)) % 128) : (pidx + 1) % 128;
			for (uint8_t i = 0, oidx = didx; i < 128 - didx; i++, oidx++) {
				uint8_t idx = (i + idx_start) % 128;
				uint8_t min = ((scale * (lpmin[idx] - pmin)) >> 25);
				uint8_t max = ((scale * (lpmax[idx] - pmin)) >> 25);
				if (lpmax[idx] - lpmin[idx] <= (global.graphModeCurrent ? 8 : 60)) {
					oled.set_pixel(oidx,
							52 - ((scale * (lpavg[idx] - pmin)) >> 25));
				} else {
					oled.line(oidx, 53 - min, oidx, 52 - max);
					if (max - min > 4) {
						oled.clear_pixel(oidx,
								52 - ((scale * (lpavg[idx] - pmin)) >> 25));
					}
				}
			}

			// stop rolling on global.overload
			if (global.overload == 0) {
				pidx = pidx < 127 ? pidx + 1 : 0;
				if (didx > 0)
					didx--;
			}

			// current mA
			printFloat(0,1,(float) (lsumBusMicroAmps / lreadings) / 1000, 4, false,"mA");

			// max value on grath
			printFloat(0,7,(float) pmin / 1000, global.graphModeCurrent ? 4 : 3, true,
					global.graphModeCurrent ? "mA" : "v");

			printFloat(9,7,(float) pmax / 1000, global.graphModeCurrent? 4 : 3, false,
					global.graphModeCurrent ? "mA" : "v");

			// mAh
			printFloat(8,1,(float) (ltotalBusMicroAmps / lnow) / 1000, 4, false,
					"mAh");
		}

		if (global.overload) {
			oled.setCursor(3, 3);
			oled.puts("overload !!");
		}

		for (uint8_t i = 0; i <= 8; i += 2) {
			for (uint8_t j = 0; j < 8 - i; j++) {
				if (i >= global.rangeScale * 2)
					oled.set_pixel(56 + j, i);
				else
					oled.clear_pixel(56 + j, i);
			}
		}
		oled.update();
	}
}
