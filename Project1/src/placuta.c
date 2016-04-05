//*****************************************************************************
//   +--+
//   | ++----+
//   +-++    |
//     |     |
//   +-+--+  |
//   | +--+--+
//   +----+    Copyright (c) 2011 Code Red Technologies Ltd.
//
// LED flashing SysTick application for LPCXPresso11U14 board
//
// Software License Agreement
//
// The software is owned by Code Red Technologies and/or its suppliers, and is
// protected under applicable copyright laws.  All rights are reserved.  Any
// use in violation of the foregoing restrictions may subject the user to criminal
// sanctions under applicable laws, as well as to civil liability for the breach
// of the terms and conditions of this license.
//
// THIS SOFTWARE IS PROVIDED "AS IS".  NO WARRANTIES, WHETHER EXPRESS, IMPLIED
// OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE APPLY TO THIS SOFTWARE.
// USE OF THIS SOFTWARE FOR COMMERCIAL DEVELOPMENT AND/OR EDUCATION IS SUBJECT
// TO A CURRENT END USER LICENSE AGREEMENT (COMMERCIAL OR EDUCATIONAL) WITH
// CODE RED TECHNOLOGIES LTD.
//
//*****************************************************************************

#ifdef __USE_CMSIS
#include "LPC11Uxx.h"
#endif

#include "LPC11Uxx.h"
#include "gpio.h"
#include "core_cm0.h"
#include"timer32.h"

#define TEST_TIMER_NUM		0		/* 0 or 1 for 32-bit timers only */

extern volatile uint32_t timer32_0_counter[4];
extern volatile uint32_t timer32_1_counter[4];

// Variable to store CRP value in. Will be placed automatically
// by the linker when "Enable Code Read Protect" selected.
// See crp.h header for more information
//__CRP const unsigned int CRP_WORD = CRP_NO_CRP ;

// LPCXpresso eval board LED

volatile int m = 0;
volatile static int i = 0;
volatile uint32_t msTicks; /* counts 1ms timeTicks */
/*----------------------------------------------------------------------------
 SysTick_Handler
 *----------------------------------------------------------------------------*/
void SysTick_Handler(void) {
	msTicks++; /* increment counter necessary in Delay() */
}

/*------------------------------------------------------------------------------
 delays number of tick Systicks (happens every 1 ms)
 *------------------------------------------------------------------------------*/
__INLINE static void Delay(uint32_t dlyTicks) {
	uint32_t curTicks;

	curTicks = msTicks;
	while ((msTicks - curTicks) < dlyTicks)
		;
}

void delay() {
	for (i = 0; i < 50000; i++)
		;
}

typedef struct {
	const uint32_t Port;
	const uint32_t Bit;
} Pin;

struct {
	const struct {
		const Pin Top;
		const Pin TopLeft;
		const Pin TopRight;
		const Pin Middle;
		const Pin BottomLeft;
		const Pin BottomRight;
		const Pin Bottom;
	} SevenSegmentDisplay;

	const struct {
		const Pin TopLeft;
		const Pin TopRight;
		const Pin BottomLeft;
		const Pin BottomRight;
	} Button;
} Constants =
	{
		{
			{ 0, 9 },  // Top
			{ 0, 5 },  // TopLeft
			{ 0, 1 },  // TopRight
			{ 0, 7 },  // Middle
			{ 0, 4 },  // BottomLeft
			{ 0, 2 },  // BottomRight
			{ 0, 3 }   // Bottom
		},
		{
			{ 0, 21 }, // TopLeft
			{ 1, 23 }, // TopRight
			{ 0, 11 }, // BottomLeft
			{ 0, 15 }  // BottomRight
		}
	};

void SetOutPin(Pin pin){
	GPIOSetDir(pin.Port, pin.Bit, 1);
}

void SetInPin(Pin pin){
	GPIOSetDir(pin.Port, pin.Bit, 0);
}

void SetDisplaySegment(Pin displayPin) {
	GPIOSetBitValue(displayPin.Port, displayPin.Bit, 0);
}

void ClearDisplaySegment(Pin displayPin) {
	GPIOSetBitValue(displayPin.Port, displayPin.Bit, 1);
}

int IsButtonPressed(Pin buttonPin){
	 return !GPIOGetPinValue(buttonPin.Port, buttonPin.Bit);
}

void Configure7SegmentDisplay() {
	SetOutPin(Constants.SevenSegmentDisplay.Top);
	SetOutPin(Constants.SevenSegmentDisplay.TopLeft);
	SetOutPin(Constants.SevenSegmentDisplay.TopRight);
	SetOutPin(Constants.SevenSegmentDisplay.Middle);
	SetOutPin(Constants.SevenSegmentDisplay.Bottom);
	SetOutPin(Constants.SevenSegmentDisplay.BottomLeft);
	SetOutPin(Constants.SevenSegmentDisplay.BottomRight);
}

void ConfigureButtons() {
	SetInPin(Constants.Button.TopLeft);
	SetInPin(Constants.Button.TopRight);
	SetInPin(Constants.Button.BottomLeft);
	SetInPin(Constants.Button.BottomRight);
}

void Clear() {
	ClearDisplaySegment(Constants.SevenSegmentDisplay.Top);
	ClearDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	ClearDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	ClearDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	ClearDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	ClearDisplaySegment(Constants.SevenSegmentDisplay.BottomLeft);
	ClearDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit0() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit1() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit2() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomLeft);
}

void SetDisplayDigit3() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit4() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit5() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit6() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit7() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit8() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit9() {
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(Constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(Constants.SevenSegmentDisplay.BottomRight);
}

void (*DisplayDigitSetters[])() =
{
	SetDisplayDigit0,
	SetDisplayDigit1,
	SetDisplayDigit2,
	SetDisplayDigit3,
	SetDisplayDigit4,
	SetDisplayDigit5,
	SetDisplayDigit6,
	SetDisplayDigit7,
	SetDisplayDigit8,
	SetDisplayDigit9
};

void SetDisplayDigit(uint8_t digit){
	Clear();
	DisplayDigitSetters[digit % 10]();
}

void SetDisplayRed(){
	SetDisplaySegment(Constants.SevenSegmentDisplay.Top);
}
void SetDisplayGreen(){
	SetDisplaySegment(Constants.SevenSegmentDisplay.Bottom);
}

int main(void) {
	GPIOInit();

	Configure7SegmentDisplay();
	ConfigureButtons();

	while (1) {
		Clear();
		SetDisplayRed();
		if (IsButtonPressed(Constants.Button.TopLeft)) {
			Clear();
			SetDisplayGreen();
			for(int i = 0; i < 7000000; i++)
				;
	}
}
