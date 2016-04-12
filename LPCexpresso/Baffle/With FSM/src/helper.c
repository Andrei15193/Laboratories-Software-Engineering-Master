#include "helper.h"
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

void (*DisplayDigitSetters[])() = {
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

const Constants constants =
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

Constants GetConstants() {
	return constants;
}

void delay() {
	for (int i = 0; i < 50000; i++)
		;
}

void SetOutPin(Pin pin) {
	GPIOSetDir(pin.Port, pin.Bit, 1);
}

void SetInPin(Pin pin) {
	GPIOSetDir(pin.Port, pin.Bit, 0);
}

void SetDisplaySegment(Pin displayPin) {
	GPIOSetBitValue(displayPin.Port, displayPin.Bit, 0);
}

void ClearDisplaySegment(Pin displayPin) {
	GPIOSetBitValue(displayPin.Port, displayPin.Bit, 1);
}

int IsButtonPressed(Pin buttonPin) {
	return !GPIOGetPinValue(buttonPin.Port, buttonPin.Bit);
}

void Configure7SegmentDisplay() {
	SetOutPin(constants.SevenSegmentDisplay.Top);
	SetOutPin(constants.SevenSegmentDisplay.TopLeft);
	SetOutPin(constants.SevenSegmentDisplay.TopRight);
	SetOutPin(constants.SevenSegmentDisplay.Middle);
	SetOutPin(constants.SevenSegmentDisplay.Bottom);
	SetOutPin(constants.SevenSegmentDisplay.BottomLeft);
	SetOutPin(constants.SevenSegmentDisplay.BottomRight);
}

void ConfigureButtons() {
	SetInPin(constants.Button.TopLeft);
	SetInPin(constants.Button.TopRight);
	SetInPin(constants.Button.BottomLeft);
	SetInPin(constants.Button.BottomRight);
}

void Clear() {
	ClearDisplaySegment(constants.SevenSegmentDisplay.Top);
	ClearDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	ClearDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	ClearDisplaySegment(constants.SevenSegmentDisplay.Middle);
	ClearDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	ClearDisplaySegment(constants.SevenSegmentDisplay.BottomLeft);
	ClearDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit0() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit1() {
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit2() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomLeft);
}

void SetDisplayDigit3() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit4() {
	SetDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit5() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit6() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit7() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit8() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit9() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopLeft);
	SetDisplaySegment(constants.SevenSegmentDisplay.TopRight);
	SetDisplaySegment(constants.SevenSegmentDisplay.Middle);
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
	SetDisplaySegment(constants.SevenSegmentDisplay.BottomRight);
}

void SetDisplayDigit(uint8_t digit) {
	Clear();
	DisplayDigitSetters[digit % 10]();
}

void SetDisplayRed() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Top);
}
void SetDisplayGreen() {
	SetDisplaySegment(constants.SevenSegmentDisplay.Bottom);
}
