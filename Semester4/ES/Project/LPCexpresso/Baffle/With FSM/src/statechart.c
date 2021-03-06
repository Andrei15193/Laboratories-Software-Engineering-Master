/* ****************************************************** */
/* State machine generated by Red State Machine generator */
/* Thu Apr 07 21:29:02 EEST 2016                          */
/* This file was automatically generated and will be      */
/* OVERWRITTEN if regenerated.                            */
/*  o Do not make changes to this file directly.          */
/*  o Do define your actions functions in                 */
/*     statechart_actions.c                               */
/* ****************************************************** */
#include "statechart.h"

static const state initialState = S_GREENSTATE;   /* The starting state : greenState */
static state currentState = S_GREENSTATE;   /* Initialize the current state. */

static const inputs * inRef; /* Const pointer to the last input sent to the dispatch function */
static outputs * outRef; /* Pointer to the outputs sent to the dispatch function */

void preload(outputs * out)
{
    out->duration = 160000;
    out->timer = 160000;
}

void statechart_dispatch(inputs theInputSignal, outputs * out)
{
    /* The inputs */
    int button = theInputSignal.button;  /* generic variable button setting type from input source. */

    /* The outputs */
    int duration = out->duration;  /*  output variable duration (type taken from variable's source field - not checked) */
    int timer = out->timer;  /*  output variable timer (type taken from variable's source field - not checked) */

    inRef = &theInputSignal; /* To give functions access to the input signal via const inputs * getInput() */
    outRef = out; /* To give functions access to the outputs via outputs * getOutput() */

    /* Check for the reset signal: reset */
    if(!timer) {
        preload(out);
        currentState = initialState;
        return;
    }
    /* Perform any actions and state transitions for the current state and input */
    switch(currentState) {
        case S_GREENSTATE:
            /* (duration == timer) */
            if((duration == timer)) {
                redOn();  /* Action: redOn */
                greenOff();  /* Action: greenOff */
                currentState = S_REDSTATE;
                return;
            }
            /* !(duration == timer) */
            if(!(duration == timer)) {
                incrementDuration();  /* Action: incrementDuration */
                return;
            }
            break;
        case S_REDSTATE:
            /* button */
            if(button) {
                redOff();  /* Action: redOff */
                greenOn();  /* Action: greenOn */
                out->duration = 0;  /* Setting duration to value. */
                currentState = S_GREENSTATE;
                return;
            }
            break;
        default:
            /* ***************** NOTE ******************** */
            /* If the current state is not recognized      */
            /* the state machine will stay in that unknown */
            /* state until it receives the reset signal    */
            /* ******************************************* */
            break;
    }
}
int getState()
{
    return currentState;
}

const inputs * getInput()
{
    return inRef;
}

outputs * getOutput()
{
    return outRef;
}

