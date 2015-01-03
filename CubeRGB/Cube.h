/*  ____        __               ______      ____                __
   / __ \__  __/ /___ _____     / ____/___  / / /___ ___  ______/ /
  / / / / / / / / __ `/ __ \   / /   / __ \/ / / __ `/ / / / __  /
 / /_/ / /_/ / / /_/ / / / /  / /___/ /_/ / / / /_/ / /_/ / /_/ /
/_____/\__, /_/\__,_/_/ /_/   \____/\____/_/_/\__,_/\__,_/\__,_/
      /____/

Fichier header : Cube.h
Date de creation : 01.09.2013
Codeur et compilateur : Code Blocks Arduino, GCC
Descrition : Contient toutes les méthodes de la classe Cube

Modifications :

------------------------------------------------------------------
Date                |Description
------------------------------------------------------------------
03.09.2013          |   Implémentation en C++ POO
                    |   Instruction pour la commande dans le .ino
25.09.2013          |   Ajout d'un define ARDUINO
_________________________________________________________________
//              INSTRUCTION POUR L'IMPLEMANTATION
-----------------------------------------------------------------

You Have to make a main.ino like that :
[Code]

Cube cube; //Creation du Cube

void setup()
{
    cube.setup();
}

//Interuption pour le fafraichissement du cube
Cube::ISR (TIMER2_COMPA_vect)
{
    cube.refresh();
}

void loop()
{
    cube.base();
}

[/Code]

_________________________________________________________________
//              Debut du fichier cube.h
-----------------------------------------------------------------
*/
#ifndef CUBE_H
#define CUBE_H

#include "cube.h"

//#define ARDUINO "UNO"
#define ARDUINO "MEGA"

class Cube
{
    public:
        Cube();
        ~Cube();
        void setup();
        void refresh(); //Rafraichissement cube
		
		void light(char rgb);

		/*
        void base();
		*/
        void feuArtifice();
        void jeuJohnConway(int = 100);
		void shiftPlaneRGB(int time);
		void sinusRotation();

		void faceTombante();

		void effect_rain(int iterations);
		/*
        //Effets divers par le Créateur
        void draw_positions_axis (char axis, unsigned char positions[64], int invert);
        void effect_boxside_randsend_parallel (char axis, int origin, int delay, int mode);
        
        void effect_random_filler (int delay, int state);
        void effect_blinky2();
        void effect_planboing (int plane, int speed);
		*/

     //private:

        unsigned char cube[8][8];
		unsigned char cubeRGB[8][8][3];


        int current_layer;
		int color;

		void setvoxel(int x, int y, int z, int rgb);
		void clrvoxel(int x, int y, int z, int rgb);

		unsigned char inrange(int x, int y, int z, int rgb);
		bool isRed(int RGB);
		bool isGreen(int RGB);
		bool isBlue(int RGB);


		void altervoxel(int x, int y, int z, int rgb);
		

        unsigned char getvoxel(int x, int y, int z, int rgb);
		/*
        void flpvoxel(int x, int y, int z);
        void argorder(int ix1, int ix2, int *ox1, int *ox2);
		*/

        void setplane_z (int z, char rgb);
		void clrplane_z (int x);
		void setplane_x(int x, char rgb);
        void clrplane_x (int x);
		void setplane_y(int y, char rgb);
        void clrplane_y (int y);
		void setplane(char axis, unsigned char i, unsigned char rgb);
        void clrplane (char axis, unsigned char i);
		
		void setPlaneAngle(char axis, char x, char y, char z, unsigned char rgb, int angle);
        
		void fill(unsigned char pattern, unsigned char rgb);
		/*
        void box_filled(int x1, int y1, int z1, int x2, int y2, int z2);
        void box_walls(int x1, int y1, int z1, int x2, int y2, int z2);
        void box_wireframe(int x1, int y1, int z1, int x2, int y2, int z2);
        char byteline (int start, int end);
        char flipbyte (char byte);
        void line(int x1, int y1, int z1, int x2, int y2, int z2);
		*/
		void shift(char axis, int direction);
		void delay_ms(int x);
};

#endif // CUBE_H
