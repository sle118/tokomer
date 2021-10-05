#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>


typedef enum {
	ACTION_NONE,
	ACTION_RESET_STATS,
	ACTION_GRAPH_SELECT_VOLTAGE,
	ACTION_GRAPH_SELECT_CURRENT,
	ACTION_SERIAL_ON,
	ACTION_SERIAL_OFF,
	ACTION_POWER_ON,
	ACTION_POWER_OFF,
	ACTION_SET_RANGE_SCALE_0,
	ACTION_SET_RANGE_SCALE_1,
	ACTION_SET_RANGE_SCALE_2,
	ACTION_SET_RANGE_SCALE_3,
	ACTION_DIGITALIN_ON,
	ACTION_DIGITALIN_OFF,
	ACTION_GET_STATUS,
	ACTION_BINARY_ON,
	ACTION_BINARY_OFF,
	ACTION_HELP
} actions_t ;
typedef enum {
	COMMAND_RESULT_OK,
	COMMAND_RESULT_UNKNOWN
} command_result_t;

typedef struct command_fifo
{
	uint32_t head;
	uint32_t tail;
	actions_t data[8]; // must be power 2^x
} command_fifo_t;

#ifdef __cplusplus
extern "C" {
#endif
void initialize_commands();
void enqueue(actions_t cmd);
bool dequeue(actions_t * cmd);
command_result_t process_inbound_serial(const char *cmd_line);
command_result_t handle_command_string(const char *cmd_string) ;
void handleCommandQueue();
void calibrate();
void USBRx(void const *arg);
#ifdef __cplusplus
}
#endif
