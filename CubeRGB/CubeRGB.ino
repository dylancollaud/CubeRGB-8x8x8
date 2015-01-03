#include <Arduino.h>
#include "Cube.h"
#include <avr/interrupt.h>
#include <HardwareSerial.h>

//#define ARDUINO 100
Cube cube; //Creation du Cube

int PinR = 37;
int PinG = 36;
int PinB = 35;

String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete

void setup()
{
	cube.setup();
	Serial.begin(57600);
}

//Interuption pour le fafraichissement du cube
ISR(TIMER2_COMPA_vect)
{
	cube.refresh();
}

void loop()
{


	int i;
	//cube.effect_rain(100);
	//cube.sinusRotation();
	//cube.shiftPlaneRGB(200);
	//cube.jeuJohnConway();
	//cube.sinusRotation();
	//cube.feuArtifice();
	//cube.faceTombante();
	/*
	for (i = 0; i < 8; i++){
		cube.light(i);
		delay(1000);
		cube.light(0);
	}*/
	
	
	//cube.light(4);
	/*
	int x = 0;
	int y = 0;
	int z = 0;
	for (x = 0; x < 8; x++)
	{
		for (y = 0; y < 8; y++){
			for (z = 0; z < 8; z++){
				cube.setvoxel(x, y, z, 1);
				delay(100);
			}
		}
	}*/

	/*
	for (i = 0; i < 8; i++)
	{
		cube.setvoxel(i, 0, 0, 1);
		delay(50);
	}*/

	//cube.light(2);
	//cube.setplane_z(0, 3);
	//cube.setvoxel(7, 2, 3, 1);

	
	
	
	double tempsDepart, tempsFin;
	char temps[196];
	unsigned char* ptCube = &cube.cubeRGB[0][0][0];
	bool synchro;
	char sync;
	while (true){
		
		while (Serial.available() < 192);
			Serial.readBytes(ptCube, 192);

		if (cube.cubeRGB[0][0][0] == 254)
		{
			//Reset cube
			Serial.write("RESET");
			delay(1000);
			while(Serial.available())
			{
				Serial.read();
			}
			cube.setvoxel(0, 0, 0, 7);
		}

	}
	
}

/*
void serialEvent() {
	char tab[50];
	int i = 0;
	while (Serial.available()) {
		// get the new byte:
		char inChar = (char)Serial.read();
		tab[i++] = inChar;
		Serial.write(inChar);

		// add it to the inputString:
		inputString += inChar;
		// if the incoming character is a newline, set a flag
		// so the main loop can do something about it:
		if (inChar == '#') {
			//sprintf(tab, "%s", inputString);
			//Serial.write(tab);
				cube.setBrightness(tab[0]-48);
		}
	}
}*/