#ifndef HELPER
#define HELPER

#ifdef __USE_CMSIS
#include "LPC11Uxx.h"
#endif

#include "LPC11Uxx.h"
#include "gpio.h"
#include "core_cm0.h"
#include "timer32.h"

void delay();

typedef struct {
	const uint32_t Port;
	const uint32_t Bit;
} Pin;

typedef struct Constants {
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
} Constants;

Constants GetConstants();

void SetOutPin(Pin pin);

void SetInPin(Pin pin);

void SetDisplaySegment(Pin displayPin);

void ClearDisplaySegment(Pin displayPin);

int IsButtonPressed(Pin buttonPin);

void Configure7SegmentDisplay();

void ConfigureButtons();

void Clear();

void SetDisplayDigit0();

void SetDisplayDigit1();

void SetDisplayDigit2();

void SetDisplayDigit3();

void SetDisplayDigit4();

void SetDisplayDigit5();

void SetDisplayDigit6();

void SetDisplayDigit7();

void SetDisplayDigit8();

void SetDisplayDigit9();

void SetDisplayDigit(uint8_t digit);

void SetDisplayRed();

void SetDisplayGreen();

#endif // HELPER
