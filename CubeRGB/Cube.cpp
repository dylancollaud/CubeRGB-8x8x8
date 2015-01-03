/*  ____        __               ______      ____                __
   / __ \__  __/ /___ _____     / ____/___  / / /___ ___  ______/ /
   / / / / / / / / __ `/ __ \   / /   / __ \/ / / __ `/ / / / __  /
   / /_/ / /_/ / / /_/ / / / /  / /___/ /_/ / / / /_/ / /_/ / /_/ /
   /_____/\__, /_/\__,_/_/ /_/   \____/\____/_/_/\__,_/\__,_/\__,_/
   /____/

   Fichier sources : Cube.cpp
   Date de creation : 01.09.2013
   Codeur et compilateur : Code Blocks Arduino, GCC
   Descrition : s'occupe de gerer le cube 3d LED

   */

#include <Arduino.h>
#include <math.h>
#include <avr/interrupt.h>
#include <Arduino.h>
#include "Cube.h"

#define AXIS_X 1
#define AXIS_Y 2
#define AXIS_Z 3



Cube::Cube()
{
	current_layer = 0;
}

Cube::~Cube()
{
	//dtor
}

void Cube::setup()
{
	int i;
	// pinMode(A0, OUTPUT) as specified in the arduino reference didn't work. So I accessed the registers directly.

	if (ARDUINO == "UNO"){
		//FOR ARDUINO UNO

		for (i = 0; i < 14; i++)
			pinMode(i, OUTPUT);

		DDRC = 0xff;
		PORTC = 0x00;

	}
	else if (ARDUINO == "MEGA"){

		for (i = 50; i <= 53; i++)
			pinMode(i, OUTPUT);
		for (i = 35; i <= 37; i++)
			pinMode(i, OUTPUT);

		//FOR ARDUINO MEGA
		DDRK = 0xff;
		DDRF = 0xff;

		PORTK = 0x00;
		PORTF = 0x00;

		PORTC &= ~(0b00000111);
		PORTC |= 1;
	}

	// Reset any PWM configuration that the arduino may have set up automagically!
	TCCR2A = 0x00;
	TCCR2B = 0x00;

	TCCR2A |= (0x01 << WGM21); // CTC mode. clear counter on TCNT2 == OCR2A
	OCR2A = 20; // Interrupt every 25600th cpu cycle (256*100)
	TCNT2 = 0x00; // start counting at 0
	TCCR2B |= (0x01 << CS22) | (0x01 << CS21); // Start the clock with a 256 prescaler

	TIMSK2 |= (0x01 << OCIE2A);

	int x, y, z, rgb;
	for (y = 0; y < 8; y++)
	for (z = 0; z < 8; z++)
	for (rgb = 0; rgb < rgb; x++)
		cubeRGB[z][y][rgb] = 0;

}
void Cube::refresh(){

	static int localIntens = 0;

	if (ARDUINO == "MEGA"){


		//FOR ARDUINO MEGA
		// FOR ARDUINO MEGA BY DYLAN COLLAUD

		char i, j = 0, k, x;
		// all layer selects off

		PORTF = 0x00;

		for (j = 0; j < 8; j++){

			PORTB |= 0x08; // output enable off.

			for (i = 0; i < 8; i++)
			{
				PORTB = (PORTB & 0xF8) | (0x07 & (i)); // défini l'adresse


				PORTK = cubeRGB[current_layer][i][j]; //Défini le bus
			}
			PORTC = (PORTC & 0xF8) | (1 << j + 1);
			//Change l'adressage de la couleur un crans en avance...

			PORTB &= 0b11110111; // Output enable on.

		}
		PORTF = (0x01 << current_layer);
		PORTC = 1;



		current_layer++;
		if (current_layer >= 8){
			current_layer = 0;
		}


	}
}

void Cube::light(char rgb){
	fill(0xff, rgb);
}



/*


void Cube::base(){
effect_planboing(AXIS_Z, 400);
effect_planboing(AXIS_Y, 400);
effect_planboing(AXIS_X, 400);

effect_blinky2();

effect_random_filler(75,1);
effect_random_filler(75,0);

effect_rain(100);

effect_boxside_randsend_parallel (AXIS_X, 0, 150, 1);
effect_boxside_randsend_parallel (AXIS_X, 1, 150, 1);
effect_boxside_randsend_parallel (AXIS_Y, 0, 150, 1);
effect_boxside_randsend_parallel (AXIS_Y, 1, 150, 1);
effect_boxside_randsend_parallel (AXIS_Z, 0, 150, 1);
effect_boxside_randsend_parallel (AXIS_Z, 1, 150, 1);
}



// ==========================================================================================
//   Effect functions
// ==========================================================================================

void Cube::draw_positions_axis(char axis, unsigned char positions[64], int invert)
{
int x, y, p;

fill(0x00);

for (x = 0; x < 8; x++)
{
for (y = 0; y < 8; y++)
{
if (invert)
{
p = (7 - positions[(x * 8) + y]);
}
else
{
p = positions[(x * 8) + y];
}

if (axis == AXIS_Z)
setvoxel(x, y, p);

if (axis == AXIS_Y)
setvoxel(x, p, y);

if (axis == AXIS_X)
setvoxel(p, y, x);
}
}

}


void Cube::effect_boxside_randsend_parallel(char axis, int origin, int delay, int mode)
{
int i;
int done;
unsigned char cubepos[64];
unsigned char pos[64];
int notdone = 1;
int notdone2 = 1;
int sent = 0;

for (i = 0; i < 64; i++)
{
pos[i] = 0;
}

while (notdone)
{
if (mode == 1)
{
notdone2 = 1;
while (notdone2 && sent < 64)
{
i = rand() % 64;
if (pos[i] == 0)
{
sent++;
pos[i] += 1;
notdone2 = 0;
}
}
}
else if (mode == 2)
{
if (sent < 64)
{
pos[sent] += 1;
sent++;
}
}

done = 0;
for (i = 0; i<64; i++)
{
if (pos[i] > 0 && pos[i] < 7)
{
pos[i] += 1;
}

if (pos[i] == 7)
done++;
}

if (done == 64)
notdone = 0;

for (i = 0; i < 64; i++)
{
if (origin == 0)
{
cubepos[i] = pos[i];
}
else
{
cubepos[i] = (7 - pos[i]);
}
}


delay_ms(delay);
draw_positions_axis(axis, cubepos, 0);

}

}
*/

void Cube::effect_rain(int iterations)
{
	int i, ii;
	int rnd_x;
	int rnd_y;
	int rnd_num;

	for (ii = 0; ii < iterations; ii++)
	{
		rnd_num = rand() % 4;

		for (i = 0; i < rnd_num; i++)
		{
			rnd_x = rand() % 8;
			rnd_y = rand() % 8;
			setvoxel(rnd_x, rnd_y, 7, 3);
		}

		delay(0);
		shift(AXIS_Z, -1);
	}
}

/*
// Set or clear exactly 512 voxels in a random order.
void Cube::effect_random_filler(int delay, int state)
{
int x, y, z;
int loop = 0;


if (state == 1)
{
fill(0x00);
}
else
{
fill(0xff);
}

while (loop < 511)
{
x = rand() % 8;
y = rand() % 8;
z = rand() % 8;

if ((state == 0 && getvoxel(x, y, z) == 0x01) || (state == 1 && getvoxel(x, y, z) == 0x00))
{
altervoxel(x, y, z, state);
delay_ms(delay);
loop++;
}
}
}

//Blink
void Cube::effect_blinky2()
{
int i, r;
fill(0x00);

for (r = 0; r<2; r++)
{
i = 750;
while (i>0)
{
fill(0x00);
delay_ms(i);

fill(0xff);
delay_ms(100);

i = i - (15 + (1000 / (i / 10)));
}

delay_ms(1000);

i = 750;
while (i > 0)
{
fill(0x00);
delay_ms(751 - i);

fill(0xff);
delay_ms(100);

i = i - (15 + (1000 / (i / 10)));
}
}

}

// Draw a plane on one axis and send it back and forth once.
void Cube::effect_planboing(int plane, int speed)
{
int i;
for (i = 0; i < 8; i++)
{
fill(0x00);
setplane(plane, i);
delay_ms(speed);
}

for (i = 7; i >= 0; i--)
{
fill(0x00);
setplane(plane, i);
delay_ms(speed);
}
}

*/
void Cube::feuArtifice()
{
	int const VMAX = 200;
	int const VMIN = -200;
	float const DIV = 100;
	float const FREIN = 1.5;
	float const GRAVITY = 0.35;//0.35

	boolean init = false;

	int x, y, z, zMax, rgbLancement;
	x = random(2, 6);
	y = random(2, 6);
	z = -1;
	rgbLancement = random(1, 7);
	zMax = random(5, 7);
	int compt = 0;

	int nbrFeu = float(random(50, 75));

	float feu[75][7];

	while (true){

		if (z != zMax){//Phase de monté
			z++;
			fill(0, 0);
			setvoxel(x, y, z, rgbLancement);
			delay(50);
		}

		if (init == false && z == zMax){ //Initialisation
			for (int i = 0; i < nbrFeu; i++){
				feu[i][0] = x;//X
				feu[i][1] = y;//Y
				feu[i][2] = z;//Z
				feu[i][3] = atan(random(VMIN, VMAX) / DIV);//vitesse X
				feu[i][4] = atan(random(VMIN, VMAX) / DIV);//vitesse Y
				feu[i][5] = atan(random(VMIN, VMAX) / DIV);//vitesse Z
				feu[i][6] = random(1, 7); // Couleur
			}

			init = true;
		}

		if (init == true){//Explosion
			//influation de la vitesse

			compt = 0;

			for (int i = 0; i < nbrFeu; i++){
				feu[i][0] += feu[i][3];
				feu[i][1] += feu[i][4];
				feu[i][2] += feu[i][5];
				feu[i][2] -= GRAVITY;
				feu[i][3] = feu[i][3] / FREIN;
				feu[i][4] = feu[i][4] / FREIN;
				feu[i][5] = feu[i][5] / FREIN;


				if ((feu[i][0] < 0 || feu[i][0] > 7) || (feu[i][1] < 0 || feu[i][1] > 7) || (feu[i][2] < 0 || feu[i][2] > 7))//S'il est dehors du cube
					compt++;
			}

			if (compt >= nbrFeu){
				fill(0, 0);
				delay(1000);
				break;
			}

			//Affichage
			fill(0, 0);
			for (int i = 0; i < nbrFeu; i++){
				setvoxel(feu[i][0], feu[i][1], feu[i][2], feu[i][6]);
				//setvoxel(feu[i][0], feu[i][1], feu[i][2], random(1,7));
			}
		}
		//if(z==zMax)
		// break;
		delay(50);
	}
}


void Cube::jeuJohnConway(int time)
{
	//Initialisation

	const int nD = 32;
	const int zone = 4;

	int x, y, z, i, j, k, xZ, yZ, zZ, l, m, n, tour, meme;

	char cubeBase[8][8][8] = {};
	char cubeFutur[8][8][8] = {};
	char cubeAncien[8][8][8] = {};

	for (i = 0; i < 8; i++){
		for (j = 0; j < 8; j++){
			for (k = 0; k < 8; k++){
				cubeBase[i][j][k] = 0;
				cubeFutur[i][j][k] = 0;
			}
		}
	}

	int cpt = 0;

	xZ = random(0, 9 - zone);
	yZ = random(0, 9 - zone);
	zZ = random(0, 9 - zone);


	for (i = 0; i < nD; i++){
		x = random(xZ, xZ + zone);
		y = random(yZ, yZ + zone);
		z = random(zZ, zZ + zone);

		cubeBase[x][y][z] = 1;
	}



	while (true){
		//Traitement du cube



		for (i = 0; i < 8; i++){
			for (j = 0; j < 8; j++){
				for (k = 0; k < 8; k++){
					cpt = 0;
					for (l = -1; l <= 1; l++){
						for (m = -1; m <= 1; m++){
							for (n = -1; n <= 1; n++){
								if ((i + l < 8 && j + m < 8 && k + n < 8) && (i + l >= 0 && j + m >= 0 && k + n >= 0) && !(l == 0 && m == 0 && n == 0)){
									if (cubeBase[i + l][j + m][k + n] == 1){
										cpt++;
									}
								}

							}
						}
					}

					if (cpt > 3 && cpt <= 5){//Règle 2
						cubeFutur[i][j][k] = cubeBase[i][j][k];
					}
					else if (cpt > 6 && cpt <= 9){//Règle 1
						cubeFutur[i][j][k] = 1;
					}
					else{ //Règle 3
						cubeFutur[i][j][k] = 0;
					}
				}
			}
		}



		//Affichage
		fill(0, 0);
		tour = 0;
		meme = 1;
		for (i = 0; i < 8; i++){
			for (j = 0; j < 8; j++){
				for (k = 0; k < 8; k++){
					if (cubeBase[i][j][k] != cubeFutur[i][j][k] && cubeAncien[i][j][k] != cubeFutur[i][j][k])
					{
						meme = 0;
					}
					if (cubeBase[i][j][k] == 1){ // BUG !!! Mettre cubeBase pour que ça tourne
						setvoxel(i, j, k, 1);
						tour++;
					}
				}
			}
		}
		if (tour == 0 || meme == 1){
			fill(0x00, 0);
			delay(100);
			break;
		}

		for (i = 0; i < 8; i++){
			for (j = 0; j < 8; j++){
				for (k = 0; k < 8; k++){
					cubeAncien[i][j][k] = cubeBase[i][j][k];
					cubeBase[i][j][k] = cubeFutur[i][j][k];
				}
			}
		}
		delay(time);
	}
}




void Cube::shiftPlaneRGB(int time)
{
	int i;

	for (i = 0; i < 8; i++){
		setplane_x(i, 1);
		setplane_y(i, 4);
		setplane_z(i, 2);
		delay(time);
		clrplane_x(i);
		clrplane_y(i);
		clrplane_z(i);
	}

	for (i = 7; i >= 0; i--){
		setplane_x(i, 1);
		setplane_y(i, 4);
		setplane_z(i, 2);
		delay(time);
		clrplane_x(i);
		clrplane_y(i);
		clrplane_z(i);
	}

}

void Cube::sinusRotation(){
	float k, temps, phi;
	int amp = 3;
	temps = 0;

	float x, y, z, a, x1, y1;

	float angleMax = M_PI * 2;
	float t[3][3] = {
		{ 1, 0, 0 },
		{ 0, 1, 0 },
		{ 0, 0, 1 } };

	int LEN = 64;//64

	float pts[LEN][3];
	int index;

	while (true){

		temps += M_PI / 36; //Vitesse
		if (temps > angleMax) temps -= angleMax;

		a += M_PI / 512; //rotation
		//a = M_PI/2; //Règle le bu de rotation
		if (a > angleMax)
			a -= angleMax;

		t[0][0] = cos(a);
		t[0][1] = -sin(a);
		t[1][0] = sin(a);
		t[1][1] = cos(a);

		//z = a * sin(t); 

		index = 0;
		phi = temps;
		for (int i = 0; i < 8; i++){

			phi += M_PI / 6; //T
			k = amp * sin(temps + phi) + 3.9;

			for (int j = 0; j < 8; j++) {

				x1 = j - 4;
				y1 = i - 4;

				pts[index][0] = t[0][0] * x1 + t[0][1] * y1 + t[0][2] * k + 4;
				pts[index][1] = t[1][0] * x1 + t[1][1] * y1 + t[1][2] * k + 4;
				pts[index][2] = t[2][0] * x1 + t[2][1] * y1 + t[2][2] * k;

				index++;
			}
		}

		fill(0, 0);
		for (int i = 0; i < LEN; i++){
			setvoxel(pts[i][0], pts[i][1], pts[i][2], pts[i][2] + 1);
		}

		delay(20);

	}

}

void Cube::faceTombante()
{
	char AXE = AXIS_X;
	int time = 50;
	int angle;
	int rgb = random(1, 7);
	for (angle = 0; angle <= 90; angle+=10){
		setPlaneAngle(AXE, 0,0,0,  rgb, -angle);
		delay(time);
	}
	rgb = random(1, 7);
	for (angle = 270; angle <= 360; angle += 10){
		setPlaneAngle(AXE, 0, 0, 7, rgb, -angle);
		delay(time);
	}
	rgb = random(1, 7);
	for (angle = 180; angle <= 270; angle += 10){
		setPlaneAngle(AXE, 0, 7, 7, rgb, -angle);
		delay(time);
	}
	rgb = random(1, 7);
	for (angle = 90; angle <= 180; angle += 10){
		setPlaneAngle(AXE, 0, 7, 0, rgb, -angle);
		delay(time);
	}

}

// ==========================================================================================
//   Draw functions
// ==========================================================================================


// Set a single voxel to ON
void Cube::setvoxel(int x, int y, int z, int rgb)
{
	if (inrange(x, y, z, rgb))
	{
		if (isRed(rgb)){
			cubeRGB[z][y][0] |= (1 << x);
		}
		if (isGreen(rgb)){
			cubeRGB[z][y][1] |= (1 << x);
		}
		if (isBlue(rgb)){
			cubeRGB[z][y][2] |= (1 << x);
		}
	}
}


// Set a single voxel to ON
void Cube::clrvoxel(int x, int y, int z, int rgb)
{
	if (inrange(x, y, z, rgb)){
		if (isRed(rgb)){
			cubeRGB[z][y][0] &= ~(1 << x);
		}
		if (isGreen(rgb)){
			cubeRGB[z][y][1] &= ~(1 << x);
		}
		if (isBlue(rgb)){
			cubeRGB[z][y][2] &= ~(1 << x);
		}
	}
}


// This function validates that we are drawing inside the cube.
unsigned char Cube::inrange(int x, int y, int z, int rgb)
{
	if (x >= 0 && x < 8 && y >= 0 && y < 8 && z >= 0 && z < 8 && rgb >= 0 && rgb <= 8)
	{
		return 0x01;
	}
	else
	{
		// One of the coordinates was outside the cube.
		return 0x00;
	}
}

bool Cube::isRed(int RGB)
{
	return RGB == 1 || RGB == 3 || RGB == 5 || RGB == 7;
}

bool Cube::isGreen(int RGB)
{
	return RGB == 2 || RGB == 3 || RGB == 6 || RGB == 7;
}

bool Cube::isBlue(int RGB)
{
	return RGB > 3;
}



// Get the current status of a voxel
unsigned char Cube::getvoxel(int x, int y, int z, int rgb)
{
	if (inrange(x, y, z, rgb))
	{
		char ret = 0;
		if (isRed(rgb) && cubeRGB[z][y][0] & (1 << x))
			ret |= (1 << 0);

		if (isGreen(rgb) && cubeRGB[z][y][1] & (1 << x))
			ret |= (1 << 1);

		if (isBlue(rgb) && cubeRGB[z][y][2] & (1 << x))
			ret |= (1 << 2);

		return ret;
	}
	else
	{
		return 0x00;
	}

}

// In some effect we want to just take bool and write it to a voxel
// this function calls the apropriate voxel manipulation function.
void Cube::altervoxel(int x, int y, int z, int rgb)
{
	if (rgb != 0)
	{
		setvoxel(x, y, z, rgb);
	}
	else
	{
		clrvoxel(x, y, z, 7);
	}
}
/*
// Flip the state of a voxel.
// If the voxel is 1, its turned into a 0, and vice versa.
void Cube::flpvoxel(int x, int y, int z)
{
if (inrange(x, y, z))
cube[z][y] ^= (1 << x);
}

// Makes sure x1 is alwas smaller than x2
// This is usefull for functions that uses for loops,
// to avoid infinite loops
void Cube::argorder(int ix1, int ix2, int *ox1, int *ox2)
{
if (ix1>ix2)
{
int tmp;
tmp = ix1;
ix1= ix2;
ix2 = tmp;
}
*ox1 = ix1;
*ox2 = ix2;
}
*/
// Sets all voxels along a X/Y plane at a given point
// on axis Z
void Cube::setplane_z(int z, char rgb)
{
	int i, c;
	if (z >= 0 && z < 8)
	{
		for (i = 0; i < 8; i++){
			if (rgb != 0){
				if (isBlue(rgb))
					cubeRGB[z][i][2] = 0xff;
				if (isGreen(rgb))
					cubeRGB[z][i][1] = 0xff;
				if (isRed(rgb))
					cubeRGB[z][i][0] = 0xff;
			}
			else
			{
				for (c = 0; c < 3; c++)
				{
					cubeRGB[z][i][c] = 0;
				}
			}
		}
	}
}

// Clears voxels in the same manner as above
void Cube::clrplane_z(int z)
{
	int i, c;
	if (z >= 0 && z < 8)
	{
		for (i = 0; i < 8; i++){
			for (c = 0; c < 3; c++)
			{
				cubeRGB[z][i][c] = 0;
			}
		}
	}
}

void Cube::setplane_x(int x, char rgb)
{
	int z;
	int y;
	int c;
	if (x >= 0 && x < 8)
	{
		for (z = 0; z < 8; z++)
		{
			for (y = 0; y < 8; y++)
			{
				if (rgb != 0){
					if (isBlue(rgb))
						cubeRGB[z][y][2] = (1 << x);
					if (isGreen(rgb))
						cubeRGB[z][y][1] = (1 << x);
					if (isRed(rgb))
						cubeRGB[z][y][0] = (1 << x);
				}
				else
				{
					for (c = 0; c < 3; c++)
					{
						cubeRGB[z][y][c] = 0;
					}
				}
			}
		}
	}
}

void Cube::clrplane_x(int x)
{
	int z;
	int y;
	int c;
	if (x >= 0 && x < 8)
	{
		for (z = 0; z < 8; z++)
		{
			for (y = 0; y < 8; y++)
			{
				for (c = 0; c < 3; c++)
				{
					cubeRGB[z][y][c] &= ~(1 << x);
				}
			}
		}
	}
}

void Cube::setplane_y(int y, char rgb)
{
	int z;
	int c;
	if (y >= 0 && y < 8)
	{
		for (z = 0; z < 8; z++){
			if (rgb != 0){
				if (isBlue(rgb))
					cubeRGB[z][y][2] = 0xff;
				if (isGreen(rgb))
					cubeRGB[z][y][1] = 0xff;
				if (isRed(rgb))
					cubeRGB[z][y][0] = 0xff;
			}
			else
			{
				for (c = 0; c < 3; c++)
				{
					cubeRGB[z][y][c] = 0;
				}
			}
		}
	}
}

void Cube::clrplane_y(int y)
{
	int z;
	int c;
	if (y >= 0 && y < 8)
	{
		for (z = 0; z < 8; z++)
		{
			for (c = 0; c < 3; c++)
			{
				cubeRGB[z][y][c] = 0;
			}
		}
	}
}

void Cube::setplane(char axis, unsigned char i, unsigned char rgb)
{
	switch (axis)
	{
	case AXIS_X:
		setplane_x(i, rgb);
		break;

	case AXIS_Y:
		setplane_y(i, rgb);
		break;

	case AXIS_Z:
		setplane_z(i, rgb);
		break;
	}
}

void Cube::clrplane(char axis, unsigned char i)
{
	switch (axis)
	{
	case AXIS_X:
		clrplane_x(i);
		break;

	case AXIS_Y:
		clrplane_y(i);
		break;

	case AXIS_Z:
		clrplane_z(i);
		break;
	}
}


void Cube::setPlaneAngle(char axis, char x, char y, char z, unsigned char rgb, int angle)
{
	float t[3][3] = {
		{ 1, 0, 0 },
		{ 0, 1, 0 },
		{ 0, 0, 1 } };

	float a;

	int LEN = 64;//64

	float pts[LEN][3];
	int index, k;

	a = -angle*M_PI / 180;

	switch (axis)
	{
	case AXIS_X:
		t[1][1] = cos(a);
		t[1][2] = -sin(a);
		t[2][1] = sin(a);
		t[2][2] = cos(a);
		break;

	case AXIS_Y:
		t[0][0] = cos(a);
		t[0][2] = sin(a);
		t[2][0] = -sin(a);
		t[2][2] = cos(a);
		break;

	case AXIS_Z:
		t[0][0] = cos(a);
		t[0][1] = -sin(a);
		t[1][0] = sin(a);
		t[1][1] = cos(a);
		break;
	}


	index = 0;
	for (int i = 0; i < 8; i++){

		k = 0;

		for (int j = 0; j < 8; j++) {


			pts[index][0] = t[0][0] * j + t[0][1] * i + t[0][2] * k+x;
			pts[index][1] = t[1][0] * j + t[1][1] * i + t[1][2] * k+y;
			pts[index][2] = t[2][0] * j + t[2][1] * i + t[2][2] * k+z;

			index++;
		}
	}

	fill(0, 0);
	for (int i = 0; i < LEN; i++){
		//Pour avoir les autres axes, changer x par y par exemple
		setvoxel(pts[i][0], pts[i][1], pts[i][2], rgb);
	}

}

// Fill a value into all 64 byts of the cube buffer
// Mostly used for clearing. fill(0x00)
// or setting all on. fill(0xff)

void Cube::fill(unsigned char pattern, unsigned char rgb)
{
	int z;
	int y;
	int c;
	for (z = 0; z < 8; z++)
	{
		for (y = 0; y < 8; y++)
		{
			if (rgb != 0){
				if (isBlue(rgb)){
					cubeRGB[z][y][2] = pattern;
				}
				if (isGreen(rgb)){
					cubeRGB[z][y][1] = pattern;
				}
				if (isRed(rgb)){
					cubeRGB[z][y][0] = pattern;
				}
			}
			else
			{
				for (c = 0; c < 3; c++)
				{
					cubeRGB[z][y][c] = 0;
				}
			}

		}
	}
}

/*

// Draw a box with all walls drawn and all voxels inside set
void Cube::box_filled(int x1, int y1, int z1, int x2, int y2, int z2)
{
int iy;
int iz;

argorder(x1, x2, &x1, &x2);
argorder(y1, y2, &y1, &y2);
argorder(z1, z2, &z1, &z2);

for (iz=z1;iz<=z2;iz++)
{
for (iy=y1;iy<=y2;iy++)
{
cube[iz][iy] |= byteline(x1,x2);
}
}

}

// Darw a hollow box with side walls.
void Cube::box_walls(int x1, int y1, int z1, int x2, int y2, int z2)
{
int iy;
int iz;

argorder(x1, x2, &x1, &x2);
argorder(y1, y2, &y1, &y2);
argorder(z1, z2, &z1, &z2);

for (iz=z1;iz<=z2;iz++)
{
for (iy=y1;iy<=y2;iy++)
{
if (iy == y1 || iy == y2 || iz == z1 || iz == z2)
{
cube[iz][iy] = byteline(x1,x2);
} else
{
cube[iz][iy] |= ((0x01 << x1) | (0x01 << x2));
}
}
}

}

// Draw a wireframe box. This only draws the corners and edges,
// no walls.
void Cube::box_wireframe(int x1, int y1, int z1, int x2, int y2, int z2)
{
int iy;
int iz;

argorder(x1, x2, &x1, &x2);
argorder(y1, y2, &y1, &y2);
argorder(z1, z2, &z1, &z2);

// Lines along X axis
cube[z1][y1] = byteline(x1,x2);
cube[z1][y2] = byteline(x1,x2);
cube[z2][y1] = byteline(x1,x2);
cube[z2][y2] = byteline(x1,x2);

// Lines along Y axis
for (iy=y1;iy<=y2;iy++)
{
setvoxel(x1,iy,z1);
setvoxel(x1,iy,z2);
setvoxel(x2,iy,z1);
setvoxel(x2,iy,z2);
}

// Lines along Z axis
for (iz=z1;iz<=z2;iz++)
{
setvoxel(x1,y1,iz);
setvoxel(x1,y2,iz);
setvoxel(x2,y1,iz);
setvoxel(x2,y2,iz);
}

}

// Returns a byte with a row of 1's drawn in it.
// byteline(2,5) gives 0b00111100
char Cube::byteline (int start, int end)
{
return ((0xff<<start) & ~(0xff<<(end+1)));
}

// Flips a byte 180 degrees.
// MSB becomes LSB, LSB becomes MSB.
char Cube::flipbyte (char byte)
{
char flop = 0x00;

flop = (flop & 0b11111110) | (0b00000001 & (byte >> 7));
flop = (flop & 0b11111101) | (0b00000010 & (byte >> 5));
flop = (flop & 0b11111011) | (0b00000100 & (byte >> 3));
flop = (flop & 0b11110111) | (0b00001000 & (byte >> 1));
flop = (flop & 0b11101111) | (0b00010000 & (byte << 1));
flop = (flop & 0b11011111) | (0b00100000 & (byte << 3));
flop = (flop & 0b10111111) | (0b01000000 & (byte << 5));
flop = (flop & 0b01111111) | (0b10000000 & (byte << 7));
return flop;
}

// Draw a line between any coordinates in 3d space.
// Uses integer values for input, so dont expect smooth animations.
void Cube::line(int x1, int y1, int z1, int x2, int y2, int z2)
{
float xy;	// how many voxels do we move on the y axis for each step on the x axis
float xz;	// how many voxels do we move on the y axis for each step on the x axis
unsigned char x,y,z;
unsigned char lasty,lastz;

// We always want to draw the line from x=0 to x=7.
// If x1 is bigget than x2, we need to flip all the values.
if (x1>x2)
{
int tmp;
tmp = x2; x2 = x1; x1 = tmp;
tmp = y2; y2 = y1; y1 = tmp;
tmp = z2; z2 = z1; z1 = tmp;
}


if (y1>y2)
{
xy = (float)(y1-y2)/(float)(x2-x1);
lasty = y2;
} else
{
xy = (float)(y2-y1)/(float)(x2-x1);
lasty = y1;
}

if (z1>z2)
{
xz = (float)(z1-z2)/(float)(x2-x1);
lastz = z2;
} else
{
xz = (float)(z2-z1)/(float)(x2-x1);
lastz = z1;
}



// For each step of x, y increments by:
for (x = x1; x<=x2;x++)
{
y = (xy*(x-x1))+y1;
z = (xz*(x-x1))+z1;
setvoxel(x,y,z);
}

}

*/
// Delay loop.
// This is not calibrated to milliseconds,
// but we had allready made to many effects using this
// calibration when we figured it might be a good idea
// to calibrate it.
/*
void Cube::delay_ms(int x)
{
	uint8_t y, z;
	for (; x > 0; x--){
		for (y = 0; y < 90; y++){
			for (z = 0; z < 6; z++){
				asm volatile ("nop");
			}
		}
	}
}*/



// Shift the entire contents of the cube along an axis
// This is great for effects where you want to draw something
// on one side of the cube and have it flow towards the other
// side. Like rain flowing down the Z axiz.
void Cube::shift(char axis, int direction)
{
	int i, j, x, y;
	int ii, iii;
	int state;

	for (i = 0; i < 8; i++)
	{
		if (direction == -1)
		{
			ii = i;
		}
		else
		{
			ii = (7 - i);
		}


		for (x = 0; x < 8; x++)
		{
			for (y = 0; y < 8; y++)
			{
				if (direction == -1)
				{
					iii = ii + 1;
				}
				else
				{
					iii = ii - 1;
				}
				for (j = 0; j < 8; j++){
					if (axis == AXIS_Z)
					{
						state = getvoxel(x, y, iii, j);
						altervoxel(x, y, ii, state);
					}

					if (axis == AXIS_Y)
					{
						state = getvoxel(x, iii, y, j);
						altervoxel(x, ii, y, state);
					}

					if (axis == AXIS_X)
					{
						state = getvoxel(iii, y, x, j);
						altervoxel(ii, y, x, state);
					}
				}
			}
		}
	}

	if (direction == -1)
	{
		i = 7;
	}
	else
	{
		i = 0;
	}

	for (x = 0; x < 8; x++)
	{
		for (y = 0; y < 8; y++)
		{
			if (axis == AXIS_Z)
				clrvoxel(x, y, i, 7);

			if (axis == AXIS_Y)
				clrvoxel(x, i, y, 7);

			if (axis == AXIS_X)
				clrvoxel(i, y, x, 7);

		}
	}
}

