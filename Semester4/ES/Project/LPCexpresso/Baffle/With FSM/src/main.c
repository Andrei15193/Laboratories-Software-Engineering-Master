/*
 ===============================================================================
 Name        : main.c
 Author      : $(author)
 Version     :
 Copyright   : $(copyright)
 Description : main definition
 ===============================================================================
 */

#if defined (__USE_LPCOPEN)
#include "board.h"
#endif

#include <cr_section_macros.h>
#include "statechart.h"

#include "helper.h"

// TODO: insert other include files here

// TODO: insert other definitions and declarations here

volatile uint32_t msTicks;
inputs input;

void SysTick_Handler(void) {
	msTicks++;
}

static void Delay(uint32_t dlyTicks) {
	SysTick_Config(50000000 / 1000); // we're operating at 50 MHz
	uint32_t curTicks;
	curTicks = msTicks;
	while ((msTicks - curTicks) < dlyTicks)
		; // wait here...
}

int main(void) {
	GPIOInit();

#if defined (__USE_LPCOPEN)
#if !defined(NO_BOARD_LIB)
	// Read clock settings and update SystemCoreClock variable
	SystemCoreClockUpdate();
	// Set up and initialize all required blocks and
	// functions related to the board hardware
	Board_Init();
	// Set the LED to the state of "On"
	Board_LED_Set(0, true);
#endif
#endif

	Configure7SegmentDisplay();
	ConfigureButtons();

	input.button = 0;
	outputs out = {0, 0};

	Clear();
	while (1) {
		input.button = IsButtonPressed(GetConstants().Button.TopLeft);
		statechart_dispatch(input, &out);
	}

	return 0;
}
