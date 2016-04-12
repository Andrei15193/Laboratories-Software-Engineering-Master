#include "helper.h"
#include "statechart.h"
#include "statechart_actions.h"

void greenOn() {
	SetDisplaySegment(GetConstants().SevenSegmentDisplay.Bottom);
}
void greenOff() {
	ClearDisplaySegment(GetConstants().SevenSegmentDisplay.Bottom);
}

void redOn() {
	SetDisplaySegment(GetConstants().SevenSegmentDisplay.Top);
}
void redOff() {
	ClearDisplaySegment(GetConstants().SevenSegmentDisplay.Top);
}

void incrementDuration() {
	outputs * out = getOutput();
	out->duration += 1;
}
