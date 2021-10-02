/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.h
  * @brief          : Header for main.c file.
  *                   This file contains the common defines of the application.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2019 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MAIN_H
#define __MAIN_H

#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f1xx_hal.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "cmsis_os.h"
#include "stdbool.h"
/* USER CODE END Includes */

/* Exported types ------------------------------------------------------------*/
/* USER CODE BEGIN ET */

#define PACKET_HEADER_START 0b1101<<4
#define PACKET_SET_BIT(dest,bitmask) dest= dest | (bitmask)
#define PACKET_CLEAR_BIT(dest,bitmask) dest &= ~(bitmask)
#define PACKET_SIGNAL_BIT_0   0
#define PACKET_SIGNAL_BIT_1   1<<1
#define PACKET_SIGNAL_BIT_2   1<<2
#define PACKET_SIGNAL_BIT_3   1<<3
#pragma pack(push,1)
#pragma pack(1)
typedef struct {
	uint8_t header;
	int16_t current;
	int16_t voltage;
	char eol;
} serial_binary_packet_t;
#pragma pack(pop)
typedef struct {
	bool gmode;
	bool serialEnable;
	bool serialBinaryEnable;
	bool overload;
	uint8_t rangeScale;
	bool digitalInputEnable;
	bool power;
} system_state_t;

/* USER CODE END ET */

/* Exported constants --------------------------------------------------------*/
/* USER CODE BEGIN EC */

/* USER CODE END EC */

/* Exported macro ------------------------------------------------------------*/
/* USER CODE BEGIN EM */

/* USER CODE END EM */

/* Exported functions prototypes ---------------------------------------------*/
void Error_Handler(void);

/* USER CODE BEGIN EFP */

/* USER CODE END EFP */

/* Private defines -----------------------------------------------------------*/
#define LED_Pin GPIO_PIN_13
#define LED_GPIO_Port GPIOC
#define DISPDC_Pin GPIO_PIN_1
#define DISPDC_GPIO_Port GPIOA
#define INA_ALERT_Pin GPIO_PIN_2
#define INA_ALERT_GPIO_Port GPIOA
#define INA_ALERT_EXTI_IRQn EXTI2_IRQn
#define DISPRES_Pin GPIO_PIN_3
#define DISPRES_GPIO_Port GPIOA
#define RANGE3_Pin GPIO_PIN_0
#define RANGE3_GPIO_Port GPIOB
#define RANGE2_Pin GPIO_PIN_1
#define RANGE2_GPIO_Port GPIOB
#define ONEKLOAD_Pin GPIO_PIN_2
#define ONEKLOAD_GPIO_Port GPIOB
#define RANGE1_Pin GPIO_PIN_10
#define RANGE1_GPIO_Port GPIOB
#define EN_VOUT_Pin GPIO_PIN_12
#define EN_VOUT_GPIO_Port GPIOB
#define KEY1_Pin GPIO_PIN_4
#define KEY1_GPIO_Port GPIOB
#define KEY2_Pin GPIO_PIN_5
#define KEY2_GPIO_Port GPIOB
#define KEY3_Pin GPIO_PIN_9
#define KEY3_GPIO_Port GPIOB

#define SCREEN_TASK_STACK 200
#define RX_TASK_STACK 75

/* USER CODE BEGIN Private defines */
extern osThreadId osUpdateScreenThreadId;
extern uint64_t lsumBusMillVolts;
extern uint64_t lsumBusMillVoltsOrig;
extern int32_t  lmaxBusMillVolts;
extern int32_t  lminBusMillVolts;
extern int64_t  lsumBusMicroAmps;
extern int32_t  lmaxBusMicroAmps;
extern int32_t  lminBusMicroAmps;
extern int64_t  lsumBusMicroAmpsOrig;
extern int64_t  ltotalBusMicroAmps;
extern uint32_t lreadings;
extern int32_t  lnow;
extern int16_t zero;
extern int64_t totalBusMicroAmps;
extern uint8_t forcedRange;
//extern uint8_t rangeScale;
//extern uint8_t overload;
//extern bool serialEnable;
//extern bool serialBinaryEnable;
extern uint16_t ranges[4];
extern uint16_t voltageK;
extern uint16_t refreshT;
//extern uint8_t power;
extern uint8_t ina226;
extern osThreadId osHandlehandleUSBDataRXId;
//extern bool digitalInputEnable;
extern bool requestedDigitalInputEnable;
#define SIGNAL_DATA_RECEIVED (int32_t) 0x01<<0
#define SIGNAL_CR_RECEIVED (int32_t) 0x01<<1
#define SIGNAL_STATE_CHANGE (int32_t) 0x01<<2
extern bool serial1Initialized;
extern system_state_t global;
/* USER CODE END Private defines */

#ifdef __cplusplus
}
#endif

#endif /* __MAIN_H */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
