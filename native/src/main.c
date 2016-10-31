// Test program for bcm2835 library
// You can only expect this to run correctly
// as root on Raspberry Pi hardware, but it will compile and run with little effect
// on other hardware
//
// Author: Mike McCauley
// Copyright (C) 2011-2013 Mike McCauley
// $Id: test.c,v 1.5 2015/03/28 05:27:32 mikem Exp $

#include "bcm2835.h"
#include <stdio.h>
#include <stdlib.h>

#define PIN RPI_GPIO_P1_12
#define PWM_CHANNEL 0
#define RANGE 1024

int main(int argc, char **argv)
{
    if (geteuid() != 0 || getenv("FAKEROOTKEY"))
    {
        fprintf(stderr, "****You need to be root to properly run this test program\n");
        return 1;
    }

    if (!bcm2835_init())
        return 1;

    bcm2835_gpio_fsel(PIN, BCM2835_GPIO_FSEL_ALT5);

    bcm2835_pwm_set_clock(BCM2835_PWM_CLOCK_DIVIDER_16);
    bcm2835_pwm_set_mode(PWM_CHANNEL, 1, 1);
    bcm2835_pwm_set_range(PWM_CHANNEL, RANGE);

    int enabled = 1;
    int counter = 0;
    while (1)
    {
        if (counter < RANGE)
        {
            bcm2835_pwm_set_data(PWM_CHANNEL, counter);
        }
        else
        {
            bcm2835_pwm_set_data(PWM_CHANNEL, RANGE * 2 - counter);
        }

        bcm2835_delay(10);
        counter += 16;

        if (counter >= RANGE * 2) {
            bcm2835_pwm_set_mode(PWM_CHANNEL, 0, 0);
            counter = 0;

            if (enabled)
            {
                bcm2835_pwm_set_mode(PWM_CHANNEL, 0, 0);
            }
            else
            {
                bcm2835_pwm_set_mode(PWM_CHANNEL, 1, 1);
            }
            enabled = !enabled;
        }
    }

    bcm2835_pwm_set_mode(PWM_CHANNEL, 0, 0);

    if (!bcm2835_close())
        return 1;

    return 0;
}
