// #define SERIALDEBUG
#ifdef SERIALDEBUG
#include <stdarg.h>
extern UART_HandleTypeDef huart1;
char debugstring[128];
int debugprintf (const char * format, ...) {
    va_list argptr;
    va_start (argptr, format);
    vsnprintf(debugstring,128,format,argptr);
    va_end(argptr);
    HAL_UART_Transmit(&huart1,(uint8_t *)debugstring,strlen(debugstring),5000);
    return 0;
}
#define _DEBUG(format, ...) debugprintf(format, __VA_ARGS__)
#else
int debugprintf (const char * format, ...)
{
	return 0;
}
#define _DEBUG(format, ...)
#endif
