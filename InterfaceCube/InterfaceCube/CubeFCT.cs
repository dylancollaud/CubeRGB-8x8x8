using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;


namespace InterfaceCube
{
    class CubeFCT
    {
        private Cube _cube;
        Random rand = new Random();
        public int x { get; set; }
        public int y { get; set; }

        public CubeFCT(Cube cube)
        {
            _cube = cube;
        }

        public void FeuArtifice()
        {
            _cube.TimeFCT = 50;
            while (true)
            {
                int VMAX = 50;
                int VMIN = -50;
                float DIV = 75;
                double FREIN = 1.2;
                double GRAVITY = 0.2;//0.35

                bool init = false;

                int x, y, z, zMax, rgbLancement;
                x = rand.Next(2, 6);
                y = rand.Next(2, 6);
                z = -1;
                rgbLancement = rand.Next(1, 7);
                zMax = rand.Next(5, 7);
                int compt = 0;

                int nbrFeu = rand.Next(50, 75);

                double[,] feu = new double[75, 7];

                while (true)
                {

                    if (z != zMax)
                    {//Phase de monté
                        z++;
                        _cube.Fill(0, 0);
                        _cube.SetVoxel(x, y, z, rgbLancement);
                        Thread.Sleep(_cube.TimeFCT);
                    }

                    if (init == false && z == zMax)
                    { //Initialisation
                        for (int i = 0; i < nbrFeu; i++)
                        {
                            feu[i, 0] = x;//X
                            feu[i, 1] = y;//Y
                            feu[i, 2] = z;//Z
                            feu[i, 3] = (rand.Next(VMIN, VMAX) / DIV);//vitesse X
                            feu[i, 4] = (rand.Next(VMIN, VMAX) / DIV);//vitesse Y
                            feu[i, 5] = (rand.Next(VMIN, VMAX) / DIV);//vitesse Z
                            feu[i, 6] = rand.Next(1, 7); // Couleur
                        }

                        init = true;
                    }

                    if (init == true)
                    {//Explosion
                        //influation de la vitesse

                        compt = 0;

                        for (int i = 0; i < nbrFeu; i++)
                        {
                            feu[i, 0] += feu[i, 3];
                            feu[i, 1] += feu[i, 4];
                            feu[i, 2] += feu[i, 5];
                            feu[i, 2] -= GRAVITY;
                            feu[i, 3] = feu[i, 3] / FREIN;
                            feu[i, 4] = feu[i, 4] / FREIN;
                            feu[i, 5] = feu[i, 5] / FREIN;


                            if ((feu[i, 0] < 0 || feu[i, 0] > 7) || (feu[i, 1] < 0 || feu[i, 1] > 7) || (feu[i, 2] < 0 || feu[i, 2] > 7))//S'il est dehors du cube
                                compt++;
                        }

                        if (compt >= nbrFeu)
                        {
                            _cube.Fill(0, 0);
                            Thread.Sleep(10 * _cube.TimeFCT);
                            break;
                        }

                        //Affichage
                        _cube.Fill(0, 0);
                        for (int i = 0; i < nbrFeu; i++)
                        {
                            _cube.SetVoxel((int)feu[i, 0], (int)feu[i, 1], (int)feu[i, 2], (int)feu[i, 6]);
                            //setvoxel(feu[i][0], feu[i][1], feu[i][2], random(1,7));
                        }
                    }
                    //if(z==zMax)
                    // break;
                    Thread.Sleep(_cube.TimeFCT);
                }
            }


        }

        public void JeuJohnConway()
        {
            //Initialisation

            const int nD = 32;
            const int zone = 4;

            _cube.TimeFCT = 100;
            while (true)
            {

                int x, y, z, i, j, k, xZ, yZ, zZ, l, m, n, tour, meme;

                int[, ,] cubeBase = new int[8, 8, 8];
                int[, ,] cubeFutur = new int[8, 8, 8];
                int[, ,] cubeAncien = new int[8, 8, 8];

                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 8; j++)
                    {
                        for (k = 0; k < 8; k++)
                        {
                            cubeBase[i, j, k] = 0;
                            cubeFutur[i, j, k] = 0;
                        }
                    }
                }

                int cpt = 0;

                xZ = rand.Next(0, 9 - zone);
                yZ = rand.Next(0, 9 - zone);
                zZ = rand.Next(0, 9 - zone);


                for (i = 0; i < nD; i++)
                {
                    x = rand.Next(xZ, xZ + zone);
                    y = rand.Next(yZ, yZ + zone);
                    z = rand.Next(zZ, zZ + zone);

                    cubeBase[x, y, z] = 1;
                }



                while (true)
                {
                    //Traitement du cube

                    for (i = 0; i < 8; i++)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            for (k = 0; k < 8; k++)
                            {
                                cpt = 0;
                                for (l = -1; l <= 1; l++)
                                {
                                    for (m = -1; m <= 1; m++)
                                    {
                                        for (n = -1; n <= 1; n++)
                                        {
                                            if ((i + l < 8 && j + m < 8 && k + n < 8) && (i + l >= 0 && j + m >= 0 && k + n >= 0) && !(l == 0 && m == 0 && n == 0))
                                            {
                                                if (cubeBase[i + l, j + m, k + n] == 1)
                                                {
                                                    cpt++;
                                                }
                                            }

                                        }
                                    }
                                }

                                if (cpt > 3 && cpt <= 5)
                                {//Règle 2
                                    cubeFutur[i, j, k] = cubeBase[i, j, k];
                                }
                                else if (cpt > 6 && cpt <= 9)
                                {//Règle 1
                                    cubeFutur[i, j, k] = 1;
                                }
                                else
                                { //Règle 3
                                    cubeFutur[i, j, k] = 0;
                                }
                            }
                        }
                    }



                    //Affichage
                    _cube.Fill(0, 0);
                    tour = 0;
                    meme = 1;
                    for (i = 0; i < 8; i++)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            for (k = 0; k < 8; k++)
                            {
                                if (cubeBase[i, j, k] != cubeFutur[i, j, k] && cubeAncien[i, j, k] != cubeFutur[i, j, k])
                                {
                                    meme = 0;
                                }
                                if (cubeBase[i, j, k] == 1)
                                { // BUG !!! Mettre cubeBase pour que ça tourne
                                    _cube.SetVoxel(i, j, k, _cube.RGB);
                                    tour++;
                                }
                            }
                        }
                    }
                    if (tour == 0 || meme == 1)
                    {
                        _cube.Fill(0x00, 0);
                        Thread.Sleep(_cube.TimeFCT);
                        break;
                    }

                    for (i = 0; i < 8; i++)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            for (k = 0; k < 8; k++)
                            {
                                cubeAncien[i, j, k] = cubeBase[i, j, k];
                                cubeBase[i, j, k] = cubeFutur[i, j, k];
                            }
                        }
                    }
                    Thread.Sleep(_cube.TimeFCT);
                }
            }
        }

        public void ShiftWall()
        {
            _cube.TimeFCT = 100;
            int i;
            while (true)
            {

                for (i = 0; i < 8; i++)
                {
                    _cube.SetPlane_x(i, 1);
                    _cube.SetPlane_y(i, 4);
                    _cube.SetPlane_z(i, 2);
                    Thread.Sleep(_cube.TimeFCT);
                    _cube.SetPlane_x(i, 0);
                    _cube.SetPlane_y(i, 0);
                    _cube.SetPlane_z(i, 0);
                }

                for (i = 7; i >= 0; i--)
                {
                    _cube.SetPlane_x(i, 1);
                    _cube.SetPlane_y(i, 4);
                    _cube.SetPlane_z(i, 2);
                    Thread.Sleep(_cube.TimeFCT);
                    _cube.SetPlane_x(i, 0);
                    _cube.SetPlane_y(i, 0);
                    _cube.SetPlane_z(i, 0);
                }
            }

        }

        public void SinusRotation()
        {
            double k, temps, phi;
            int amp = 3;
            temps = 0;

            double y, a = 0, x1, y1;

            double angleMax = Math.PI * 2;
            double[,] t = new double[3, 3]{
		        { 1, 0, 0 },
		        { 0, 1, 0 },
		        { 0, 0, 1 } };

            int LEN = 64;//64

            double[,] pts = new double[LEN, 3];
            int index;

            while (true)
            {

                temps += Math.PI / 12;      //Vitesse
                if (temps > angleMax) temps -= angleMax;

                a += Math.PI / 512;         //rotation
                //a = M_PI/2; //Règle le bu de rotation
                if (a > angleMax)
                    a -= angleMax;

                t[0, 0] = Math.Cos(a);
                t[0, 1] = -Math.Sin(a);
                t[1, 0] = Math.Sin(a);
                t[1, 1] = Math.Cos(a);

                index = 0;
                phi = temps;
                for (int i = 0; i < 8; i++)
                {

                    phi += Math.PI / 6; //T
                    k = amp * Math.Sin(temps + phi) + 3.9;

                    for (int j = 0; j < 8; j++)
                    {

                        x1 = j - 4;
                        y1 = i - 4;

                        pts[index, 0] = t[0, 0] * x1 + t[0, 1] * y1 + t[0, 2] * k + 4;
                        pts[index, 1] = t[1, 0] * x1 + t[1, 1] * y1 + t[1, 2] * k + 4;
                        pts[index, 2] = t[2, 0] * x1 + t[2, 1] * y1 + t[2, 2] * k;

                        index++;
                    }
                }

                _cube.Fill(0, 0);
                for (int i = 0; i < LEN; i++)
                {
                    _cube.SetVoxel((int)pts[i, 0], (int)pts[i, 1], (int)pts[i, 2], (int)pts[i, 2] + 1);
                }

                Thread.Sleep(_cube.TimeFCT);
            }
        }

        public void FaceTombante()
        {
            _cube.TimeFCT = 50;
            while (true)
            {
                Cube.Axis AXE = Cube.Axis.Axis_X;
                int angle;
                int rgb = rand.Next(1, 7);
                for (angle = 0; angle <= 90; angle += 10)
                {
                    _cube.Fill(0, 0);
                    _cube.SetPlaneAngle(AXE, 0, 0, 0, rgb, -angle);
                    Thread.Sleep(_cube.TimeFCT);
                }
                rgb = rand.Next(1, 7);
                for (angle = 270; angle <= 360; angle += 10)
                {
                    _cube.Fill(0, 0);
                    _cube.SetPlaneAngle(AXE, 0, 0, 7, rgb, -angle);
                    Thread.Sleep(_cube.TimeFCT);
                }
                rgb = rand.Next(1, 7);
                for (angle = 180; angle <= 270; angle += 10)
                {
                    _cube.Fill(0, 0);
                    _cube.SetPlaneAngle(AXE, 0, 7, 7, rgb, -angle);
                    Thread.Sleep(_cube.TimeFCT);
                }
                rgb = rand.Next(1, 7);
                for (angle = 90; angle <= 180; angle += 10)
                {
                    _cube.Fill(0, 0);
                    _cube.SetPlaneAngle(AXE, 0, 7, 0, rgb, -angle);
                    Thread.Sleep(_cube.TimeFCT);
                }
            }
        }

        public void Math3D()
        {
            _cube.TimeFCT = 50;
            double x, y, z, offset = 3.5;
            int i;
            while (true)
            {

                for (i = 1; i < 8; i++)
                {
                    _cube.Fill(0, 0);
                    for (x = 0; x < 8; x++)
                    {
                        for (y = 0; y < 8; y++)
                        {

                            z = (i * (x - offset) * (x - offset) + i * (y - offset) * (y - offset)) / 48 + offset;
                            _cube.SetVoxel((int)x, (int)y, (int)z, _cube.RGB);
                        }
                    }
                    Thread.Sleep(_cube.TimeFCT);
                }

                for (i = 7; i >= 0; i--)
                {
                    _cube.Fill(0, 0);
                    for (x = 0; x < 8; x++)
                    {
                        for (y = 0; y < 8; y++)
                        {

                            z = (i * (x - offset) * (x - offset) + i * (y - offset) * (y - offset)) / 48 + offset;
                            _cube.SetVoxel((int)x, (int)y, (int)z, _cube.RGB);
                        }
                    }
                    Thread.Sleep(_cube.TimeFCT);
                }


                for (i = 1; i < 8; i++)
                {
                    _cube.Fill(0, 0);
                    for (x = 0; x < 8; x++)
                    {
                        for (y = 0; y < 8; y++)
                        {

                            z = (-i * (x - offset) * (x - offset) + -i * (y - offset) * (y - offset)) / 48 + offset;
                            _cube.SetVoxel((int)x, (int)y, (int)z, _cube.RGB);
                        }
                    }
                    Thread.Sleep(_cube.TimeFCT);
                }

                for (i = 7; i >= 0; i--)
                {
                    _cube.Fill(0, 0);
                    for (x = 0; x < 8; x++)
                    {
                        for (y = 0; y < 8; y++)
                        {

                            z = (-i * (x - offset) * (x - offset) + -i * (y - offset) * (y - offset)) / 48 + offset;
                            _cube.SetVoxel((int)x, (int)y, (int)z, _cube.RGB);
                        }
                    }
                    Thread.Sleep(_cube.TimeFCT);
                }
            }
        }

        public void EffectRain()
        {
            int i;
            int rnd_x;
            int rnd_y;
            int rnd_num;
            _cube.TimeFCT = 100;
            while (true)
            {
                rnd_num = rand.Next() % 4;

                for (i = 0; i < rnd_num; i++)
                {
                    rnd_x = rand.Next() % 8;
                    rnd_y = rand.Next() % 8;
                    _cube.SetVoxel(rnd_x, rnd_y, 7, _cube.RGB);
                }

                Thread.Sleep(_cube.TimeFCT);
                _cube.Shift(Cube.Axis.Axis_Z, -1);
            }
        }

        public void EffectSlide()
        {
            byte[,] outside = new byte[8, 8];
            bool endSlide = false;
            Cube.Axis axe = Cube.Axis.Axis_X;
            int direction = 1;

            int i, j, x, y, z, rgb = 4, state;

            _cube.TimeFCT = 100;
            _cube.RGB = 4;

            while (true)
            {

                for (int foraxe = 0; foraxe < 3; foraxe++)
                {
                    axe = (Cube.Axis)foraxe;

                    _cube.Fill(0, 0);

                    //Select a limit
                    for (i = 0; i < 8; i++)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            outside[i, j] = (byte)rand.Next(0, 8);
                        }
                    }

                    if (axe == Cube.Axis.Axis_X)
                    {
                        if (direction == 1)
                            _cube.SetPlane_x(0, _cube.RGB);
                        else
                            _cube.SetPlane_x(7, _cube.RGB);
                    }
                    else if (axe == Cube.Axis.Axis_Y)
                    {
                        if (direction == 1)
                            _cube.SetPlane_y(0, _cube.RGB);
                        else
                            _cube.SetPlane_y(7, _cube.RGB);
                    }
                    else if (axe == Cube.Axis.Axis_Z)
                    {
                        if (direction == 1)
                            _cube.SetPlane_z(0, _cube.RGB);
                        else
                            _cube.SetPlane_z(7, _cube.RGB);
                    }

                    endSlide = false;
                    while (!endSlide)
                    {
                        endSlide = true;
                        for (x = 0; x < 8; x++)
                        {
                            for (y = 0; y < 8; y++)
                            {
                                for (z = 0; z < 8; z++)
                                {
                                    for (rgb = 0; rgb < 8; rgb++)
                                    {
                                        if (axe == Cube.Axis.Axis_X)
                                        {
                                            if ((state = _cube.GetVoxel(x, y, z, rgb)) > 0)
                                            {
                                                if (x < outside[y, z])
                                                {
                                                    _cube.ShiftPixel(Cube.Axis.Axis_X, direction, x, y, z);
                                                    endSlide = false;
                                                }
                                            }
                                        }
                                        else if (axe == Cube.Axis.Axis_Y)
                                        {
                                            if ((state = _cube.GetVoxel(z, x, y, rgb)) > 0)
                                            {
                                                if (x < outside[y, z])
                                                {
                                                    _cube.ShiftPixel(Cube.Axis.Axis_Y, direction, z, x, y);
                                                    endSlide = false;
                                                }
                                            }
                                        }
                                        else if (axe == Cube.Axis.Axis_Z)
                                        {
                                            if ((state = _cube.GetVoxel(y, z, x, rgb)) > 0)
                                            {
                                                if (x < outside[y, z])
                                                {
                                                    _cube.ShiftPixel(Cube.Axis.Axis_Z, direction, y, z, x);
                                                    endSlide = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Thread.Sleep(_cube.TimeFCT);
                        }
                    }

                    endSlide = false;
                    while (!endSlide)
                    {
                        endSlide = true;
                        for (x = 0; x < 8; x++)
                        {

                            for (y = 0; y < 8; y++)
                            {
                                for (z = 0; z < 8; z++)
                                {
                                    for (rgb = 0; rgb < 8; rgb++)
                                    {
                                        if (axe == Cube.Axis.Axis_X)
                                        {
                                            if ((state = _cube.GetVoxel(x, y, z, rgb)) > 0)
                                            {
                                                if (x < 7)
                                                {
                                                    _cube.ShiftPixel(Cube.Axis.Axis_X, direction, x, y, z);
                                                    endSlide = false;
                                                }
                                            }
                                        }
                                        else if (axe == Cube.Axis.Axis_Y)
                                        {
                                            if ((state = _cube.GetVoxel(z, x, y, rgb)) > 0)
                                            {
                                                if (x < 7)
                                                {
                                                    _cube.ShiftPixel(Cube.Axis.Axis_Y, direction, z, x, y);
                                                    endSlide = false;
                                                }
                                            }
                                        }
                                        else if (axe == Cube.Axis.Axis_Z)
                                        {
                                            if ((state = _cube.GetVoxel(y, z, x, rgb)) > 0)
                                            {
                                                if (x < 7)
                                                {
                                                    _cube.ShiftPixel(Cube.Axis.Axis_Z, direction, y, z, x);
                                                    endSlide = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Thread.Sleep(_cube.TimeFCT);
                        }
                    }
                }
            }
        }

        public void EffectBox()
        {
            _cube.TimeFCT = 50;
            while (true)
            {
                int angle;
                for (angle = 180; angle >= 90; angle -= 10)
                {
                    _cube.Fill(0, 0);

                    _cube.SetPlane_x(0, _cube.RGB);
                    _cube.SetPlane_x(7, _cube.RGB);
                    _cube.SetPlane_y(0, _cube.RGB);
                    _cube.SetPlane_y(7, _cube.RGB);
                    _cube.SetPlane_z(0, _cube.RGB);
                    _cube.SetPlaneAngle(Cube.Axis.Axis_X, 0, 7, 7, _cube.RGB, angle);
                    Thread.Sleep(_cube.TimeFCT);
                }

                for (angle = 270; angle >= 180; angle -= 10)
                {
                    _cube.Fill(0, 0);

                    _cube.SetPlane_x(0, _cube.RGB);
                    _cube.SetPlane_x(7, _cube.RGB);
                    _cube.SetPlane_y(0, _cube.RGB);
                    //_cube.SetPlane_y(7, _cube.RGB);
                    _cube.SetPlane_z(0, _cube.RGB);
                    _cube.SetPlaneAngle(Cube.Axis.Axis_X, 0, 7, 0, _cube.RGB, angle);
                    Thread.Sleep(_cube.TimeFCT);
                }

                for (angle = 270; angle <= 360; angle += 10)
                {
                    _cube.Fill(0, 0);

                    _cube.SetPlane_x(0, _cube.RGB);
                    _cube.SetPlane_x(7, _cube.RGB);
                    //_cube.SetPlane_y(0, _cube.RGB);
                    //_cube.SetPlane_y(7, _cube.RGB);
                    _cube.SetPlane_z(0, _cube.RGB);
                    _cube.SetPlaneAngle(Cube.Axis.Axis_X, 0, 0, 0, _cube.RGB, angle);
                    Thread.Sleep(_cube.TimeFCT);
                }

                for (angle = 270; angle <= 360; angle += 10)
                {
                    _cube.Fill(0, 0);

                    //_cube.SetPlane_x(0, _cube.RGB);
                    _cube.SetPlane_x(7, _cube.RGB);
                    //_cube.SetPlane_y(0, _cube.RGB);
                    //_cube.SetPlane_y(7, _cube.RGB);
                    _cube.SetPlane_z(0, _cube.RGB);
                    _cube.SetPlaneAngle(Cube.Axis.Axis_Y, 0, 0, 0, _cube.RGB, -angle);
                    Thread.Sleep(_cube.TimeFCT);
                }

                for (angle = 270; angle >= 180; angle -= 10)
                {
                    _cube.Fill(0, 0);

                    //_cube.SetPlane_x(0, _cube.RGB);
                    //_cube.SetPlane_x(7, _cube.RGB);
                    //_cube.SetPlane_y(0, _cube.RGB);
                    //_cube.SetPlane_y(7, _cube.RGB);
                    _cube.SetPlane_z(0, _cube.RGB);
                    _cube.SetPlaneAngle(Cube.Axis.Axis_Y, 7, 0, 0, _cube.RGB, -angle);
                    Thread.Sleep(_cube.TimeFCT);
                }
                _cube.Fill(0, 0);
                Thread.Sleep(10 * _cube.TimeFCT);
            }
        }

        public void Snake()
        {
            _cube.RGB = 4;
            _cube.TimeFCT = 100;
            while (true)
            {
                _cube.SetVoxel(x, y, 7, _cube.RGB);
                Thread.Sleep(_cube.TimeFCT);
                _cube.Shift(Cube.Axis.Axis_Z, -1);
            }
        }

        public void SnakeRandom()
        {

            _cube.RGB = 1;
            _cube.TimeFCT = 75;
            int nbrLed = 20;
            Point3D[] Snake = new Point3D[nbrLed];

            int tete = 0, precedant = nbrLed - 1;

            //Start
            Snake[nbrLed - 1].X = rand.Next(0, 7);
            Snake[nbrLed - 1].Y = rand.Next(0, 7);
            Snake[nbrLed - 1].Z = rand.Next(0, 7);

            while (true)
            {
                Snake[tete].X = rand.Next((int)Snake[precedant].X - 1, (int)Snake[precedant].X + 2);
                Snake[tete].Y = rand.Next((int)Snake[precedant].Y - 1, (int)Snake[precedant].Y + 2);
                Snake[tete].Z = rand.Next((int)Snake[precedant].Z - 1, (int)Snake[precedant].Z + 2);

                if ((int)Snake[tete].X < 0 || (int)Snake[tete].X > 7)
                {
                    Snake[tete].X = ((int)Snake[tete].X > 7) ? ((int)Snake[tete].X - 1) : ((int)Snake[tete].X + 1);
                }

                if ((int)Snake[tete].Y < 0 || (int)Snake[tete].Y > 7)
                {
                    Snake[tete].Y = ((int)Snake[tete].Y > 7) ? ((int)Snake[tete].Y - 1) : ((int)Snake[tete].Y + 1);
                }

                if ((int)Snake[tete].Z < 0 || (int)Snake[tete].Z > 7)
                {
                    Snake[tete].Z = ((int)Snake[tete].Z > 7) ? ((int)Snake[tete].Z - 1) : ((int)Snake[tete].Z + 1);
                }

                precedant = tete;
                tete++;
                if (tete >= nbrLed)
                    tete = 0;


                _cube.Fill(0, 0);
                for (int i = 0; i < nbrLed; i++)
                {
                    _cube.SetVoxel((int)Snake[i].X, (int)Snake[i].Y, (int)Snake[i].Z, _cube.RGB);
                }
                Thread.Sleep(_cube.TimeFCT);

            }
        }

        public void martin()
        {

            _cube.RGB = 1;
            _cube.TimeFCT = 75;
            int i = 0;
            bool monte = true;
            while (true)
            {
                if (monte)
                {
                    _cube.SetVoxel(i, i, i, _cube.RGB);
                    i++;
                    if (i == 9)
                        monte = false;
                }
                else
                {
                    _cube.ClrVoxel(i, i, i, _cube.RGB);
                    i--;
                    if (i == -1)
                        monte = true;
                }

                Thread.Sleep(_cube.TimeFCT);
             }
        }
    }
}
