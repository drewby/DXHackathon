#include "BME280.h"
#include <stdio.h>
#include <wiringPi.h>

// LED Pin - wiringPi pin 0 is BCM_GPIO 17.
// we have to use BCM numbering when initializing with wiringPiSetupSys
// when choosing a different pin number please use the BCM numbering, also
// update the Property Pages - Build Events - Remote Post-Build Event command 
// which uses gpio export for setup for wiringPiSetupSys
//#define	LED	17

int main(void)
{
	int response = bme280_init(0);
	float tempC = 0;
	float pressure = 0;
	float humidity = 0;

	while (true)
	{
		bme280_read_sensors(&tempC, &pressure, &humidity);
		printf("Temperature (C): %f \n", tempC);
		printf("Pressure: %f \n", pressure);
		printf("Humidity: %f \n", humidity);
		delay(2000);
	
	}
	return 0;
}