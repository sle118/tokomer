#pragma once

// #define SERIALDEBUG
#ifdef SERIALDEBUG
#include <stdarg.h>
extern UART_HandleTypeDef huart1;
#define _DEBUG(format, ...) debugprintf(format, __VA_ARGS__)
#else
#define _DEBUG(format, ...)
#endif
