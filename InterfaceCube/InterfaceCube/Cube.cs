using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace InterfaceCube
{
    public class Cube
    {
        private MainWindow _window;
        private byte[] _cube = new byte[192];
        private Serial _serial;
        private Thread _threadRefresh, _threadProcess;
        private bool _refreshBoucle;
        public delegate void DelFct();
        public int TimeFCT { get; set; }
        public int RGB { get; set; }

        private DispatcherTimer _refreshWindow;

        public enum Axis { Axis_X, Axis_Y, Axis_Z };

        public Cube(MainWindow window)
        {
            _window = window;
            TimeFCT = 100;
            RGB = 1;
            _serial = new Serial();
            //ConnectSerial();

            _refreshWindow = new DispatcherTimer();
            _refreshWindow.Interval = TimeSpan.FromMilliseconds(1000);
            _refreshWindow.Tick += refreshWindow;
            _refreshWindow.IsEnabled = true;

        }

        private void refreshWindow(object sender, EventArgs e)
        {

            if (!_serial.IsConnected())
            {
                _window.Button_ConnectSerial.IsEnabled = true;
                _window.Button_ResetCube.IsEnabled = false;
            }
            else
            {
                _window.Button_ConnectSerial.IsEnabled = false;
                if (_threadRefresh == null || !_threadRefresh.IsAlive)
                    StartRefresh();

                if (_threadRefresh != null && _threadRefresh.IsAlive)
                {
                    _window.Button_ResetCube.IsEnabled = true;
                }
                else
                {
                    _window.Button_ResetCube.IsEnabled = false;
                }
            }
        }

        public bool ConnectSerial(int com)
        {
            _serial.NumCom = com;
            _serial.StartConnect();
            Thread.Sleep(300);
            return true;
        }

        public void DisconnectSerial()
        {
            _serial.Disconnect();
        }

        public void ResetCube()
        {
            if (_serial.SerialPort.IsOpen)
            {
                //reset
                Fill(254, 7);
                if (!_serial.Write(_cube, 0, 192)) { return; }
                Thread.Sleep(5);
                if (!_serial.Write(_cube, 0, 192)) { return; }
                Thread.Sleep(1000);
                Fill(0, 0);
                if (!_serial.Write(_cube, 0, 192)) { return; }

            }

        }

        public bool StartRefresh()
        {

            if (_threadRefresh != null && _threadRefresh.IsAlive)
                _threadRefresh.Abort();

            ResetCube();

            _threadRefresh = new Thread(new ThreadStart(_refresh));
            _threadRefresh.Start();

            _threadRefresh.Priority = ThreadPriority.Highest;
            return true;
        }

        public bool StartProcess(DelFct del)
        {

            if (_threadRefresh != null && !_threadRefresh.IsAlive)
            {
                _threadRefresh = new Thread(new ThreadStart(_refresh));
                _threadRefresh.Start();
            }

            if (_threadProcess != null && _threadProcess.IsAlive)
                _threadProcess.Abort();

            _threadProcess = new Thread(new ThreadStart(del));
            _threadProcess.Start();

            return true;
        }

        private void _refresh()
        {
            _refreshBoucle = true;
            while (_refreshBoucle)
            {
                if (_serial.SerialPort.IsOpen)
                {
                    if (_serial.Write(_cube, 0, 192))
                    {
                        Thread.Sleep(40);
                    }
                    else
                    {
                        MessageBox.Show("Cube with COM" + _serial.NumCom + " disconnected !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _refreshBoucle = false;
                        _serial.Disconnect();
                        _serial.StartConnect();
                    }
                }
                else
                {
                    _refreshBoucle = false;
                }
            }
        }

        public void Process()
        {

        }

        public bool IsStarted()
        {
            return _threadRefresh.IsAlive && _serial.SerialPort.IsOpen;
        }

        public void CloseProcess()
        {
            try
            {
                if (_threadProcess != null)
                    _threadProcess.Abort();
                else
                    Console.WriteLine("ThreadProcess NULl");
                Fill(0, 0);
            }
            catch (Exception)
            {
                Console.WriteLine("Error process close !");
            }
        }

        public void CloseRefresh()
        {
            try
            {
                Fill(0, 0);
                Thread.Sleep(50);
                if (_threadRefresh != null)
                    _threadRefresh.Abort();
                else
                    Console.WriteLine("ThreadRefresh NULl");
            }
            catch (Exception)
            {
                Console.WriteLine("Error refresh close !");
            }
        }

        public void Close()
        {
            _refreshWindow.Stop();
            _refreshWindow.IsEnabled = false;
            CloseProcess();
            Thread.Sleep(100);
            CloseRefresh();
            _refreshBoucle = false;
            _serial.Disconnect();
        }

        public void SetVoxel(int x, int y, int z, int rgb)
        {
            if (inRange(x, y, z, rgb))
            {
                if (isRed(rgb))
                    _cube[y * 3 + z * 24] |= (byte)(1 << x);
                if (isGreen(rgb))
                    _cube[y * 3 + z * 24 + 1] |= (byte)(1 << x);
                if (isBlue(rgb))
                    _cube[y * 3 + z * 24 + 2] |= (byte)(1 << x);
            }
            else
            {
                ClrVoxel(x, y, z, 0);
            }
        }

        public void ClrVoxel(int x, int y, int z, int rgb)
        {
            if (inRange(x, y, z, rgb))
            {
                if (isRed(rgb))
                {
                    _cube[z * 24 + y * 3] &= (byte)~(1 << x);
                }
                if (isGreen(rgb))
                {
                    _cube[z * 24 + y * 3 + 1] &= (byte)~(1 << x);
                }
                if (isBlue(rgb))
                {
                    _cube[z * 24 + y * 3 + 2] &= (byte)~(1 << x);
                }
            }
        }

        public int GetVoxel(int x, int y, int z, int rgb)
        {
            if (inRange(x, y, z, rgb))
            {
                int ret = 0;
                if (isRed(rgb) && ((_cube[z * 24 + y * 3 + 0] & (byte)(1 << x)) > 0))
                    ret |= (1 << 0);

                if (isGreen(rgb) && ((_cube[z * 24 + y * 3 + 1] & (byte)(1 << x)) > 0))
                    ret |= (1 << 1);

                if (isBlue(rgb) && ((_cube[z * 24 + y * 3 + 2] & (byte)(1 << x)) > 0))
                    ret |= (1 << 2);

                return ret;
            }
            else
            {
                return 0x00;
            }

        }

        private void alterVoxel(int x, int y, int z, int rgb)
        {
            if (rgb != 0)
            {
                SetVoxel(x, y, z, rgb);
            }
            else
            {
                ClrVoxel(x, y, z, 7);
            }
        }

        /* Check if x,y,z or rgb is in the range of the cube */
        private bool inRange(int x, int y, int z, int rgb)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8 && z >= 0 && z < 8 && rgb >= 0 && rgb <= 8;
        }

        /* True if rgb is red active */
        private bool isRed(int rgb)
        {
            return rgb == 1 || rgb == 3 || rgb == 5 || rgb == 7;
        }

        /* True if rgb is green active */
        private bool isGreen(int rgb)
        {
            return rgb == 2 || rgb == 3 || rgb == 6 || rgb == 7;
        }

        /* True if rgb is blue active */
        private bool isBlue(int rgb)
        {
            return rgb > 3;
        }

        public void SetPlane_x(int x, int rgb)
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
                        if (rgb != 0)
                        {
                            if (isBlue(rgb))
                                _cube[z * 24 + y * 3 + 2] |= (byte)(1 << x);
                            if (isGreen(rgb))
                                _cube[z * 24 + y * 3 + 1] |= (byte)(1 << x);
                            if (isRed(rgb))
                                _cube[z * 24 + y * 3 + 0] |= (byte)(1 << x);
                        }
                        else
                        {
                            for (c = 0; c < 3; c++)
                            {
                                _cube[z * 24 + y * 3 + c] = 0;
                            }
                        }
                    }
                }
            }
        }

        public void SetPlane_y(int y, int rgb)
        {
            int z;
            int c;
            if (y >= 0 && y < 8)
            {
                for (z = 0; z < 8; z++)
                {
                    if (rgb != 0)
                    {
                        if (isBlue(rgb))
                            _cube[z * 24 + y * 3 + 2] = 0xff;
                        if (isGreen(rgb))
                            _cube[z * 24 + y * 3 + 1] = 0xff;
                        if (isRed(rgb))
                            _cube[z * 24 + y * 3 + 0] = 0xff;
                    }
                    else
                    {
                        for (c = 0; c < 3; c++)
                        {
                            _cube[z * 24 + y * 3 + c] = 0;
                        }
                    }
                }
            }
        }

        public void SetPlane_z(int z, int rgb)
        {
            int i, c;
            if (z >= 0 && z < 8)
            {
                for (i = 0; i < 8; i++)
                {
                    if (rgb != 0)
                    {
                        if (isBlue(rgb))
                            _cube[z * 24 + i * 3 + 2] = 0xff;
                        if (isGreen(rgb))
                            _cube[z * 24 + i * 3 + 1] = 0xff;
                        if (isRed(rgb))
                            _cube[z * 24 + i * 3 + 0] = 0xff;
                    }
                    else
                    {
                        for (c = 0; c < 3; c++)
                        {
                            _cube[z * 24 + i * 3 + c] = 0;
                        }
                    }
                }
            }
        }

        public void SetPlaneAngle(Axis axis, int x, int y, int z, int rgb, int angle)
        {
            double[,] t = new double[3, 3]{
		        { 1, 0, 0 },
		        { 0, 1, 0 },
		        { 0, 0, 1 } };

            double a;

            int LEN = 64;//64

            double[,] pts = new double[LEN, 3];
            int index, k;

            a = -angle * Math.PI / 180;

            switch (axis)
            {
                case Axis.Axis_X:
                    t[1, 1] = Math.Cos(a);
                    t[1, 2] = -Math.Sin(a);
                    t[2, 1] = Math.Sin(a);
                    t[2, 2] = Math.Cos(a);
                    break;

                case Axis.Axis_Y:
                    t[0, 0] = Math.Cos(a);
                    t[0, 2] = Math.Sin(a);
                    t[2, 0] = -Math.Sin(a);
                    t[2, 2] = Math.Cos(a);
                    break;

                case Axis.Axis_Z:
                    t[0, 0] = Math.Cos(a);
                    t[0, 1] = -Math.Sin(a);
                    t[1, 0] = Math.Sin(a);
                    t[1, 1] = Math.Cos(a);
                    break;
            }


            index = 0;
            for (int i = 0; i < 8; i++)
            {

                k = 0;

                for (int j = 0; j < 8; j++)
                {


                    pts[index, 0] = t[0, 0] * j + t[0, 1] * i + t[0, 2] * k + x;
                    pts[index, 1] = t[1, 0] * j + t[1, 1] * i + t[1, 2] * k + y;
                    pts[index, 2] = t[2, 0] * j + t[2, 1] * i + t[2, 2] * k + z;

                    index++;
                }
            }

            
            for (int i = 0; i < LEN; i++)
            {
                //Pour avoir les autres axes, changer x par y par exemple
                SetVoxel((int)pts[i, 0], (int)pts[i, 1], (int)pts[i, 2], rgb);
            }

        }

        public void Fill(int pattern, int rgb)
        {
            int z;
            int y;
            int c;
            for (z = 0; z < 8; z++)
            {
                for (y = 0; y < 8; y++)
                {
                    if (rgb != 0)
                    {
                        if (isBlue(rgb))
                        {
                            _cube[z * 24 + y * 3 + 2] = (byte)pattern;
                        }
                        if (isGreen(rgb))
                        {
                            _cube[z * 24 + y * 3 + 1] = (byte)pattern;
                        }
                        if (isRed(rgb))
                        {
                            _cube[z * 24 + y * 3] = (byte)pattern;
                        }
                    }
                    else
                    {
                        for (c = 0; c < 3; c++)
                        {
                            _cube[z * 24 + y * 3 + c] = 0;
                        }
                    }

                }
            }
        }

        public void Shift(Axis axis, int direction)
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
                        for (j = 0; j < 8; j++)
                        {
                            if (axis == Axis.Axis_Z)
                            {
                                state = GetVoxel(x, y, iii, j);
                                alterVoxel(x, y, ii, state);
                            }

                            if (axis == Axis.Axis_Y)
                            {
                                state = GetVoxel(x, iii, y, j);
                                alterVoxel(x, ii, y, state);
                            }

                            if (axis == Axis.Axis_X)
                            {
                                state = GetVoxel(iii, y, x, j);
                                alterVoxel(ii, y, x, state);
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
                    if (axis == Axis.Axis_Z)
                        ClrVoxel(x, y, i, 7);

                    if (axis == Axis.Axis_Y)
                        ClrVoxel(x, i, y, 7);

                    if (axis == Axis.Axis_X)
                        ClrVoxel(i, y, x, 7);

                }
            }
        }

        public void ShiftPixel(Axis axe, int direction, int x, int y, int z)
        {
            int state;
            for (int rgb = 0; rgb < 8; rgb++)
            {

                state = GetVoxel(x, y, z, rgb);
                if (axe == Axis.Axis_X)
                {
                    alterVoxel(x + direction, y, z, state);
                }
                else if (axe == Axis.Axis_Y)
                {
                    alterVoxel(x, y + direction, z, state);
                }
                else if (axe == Axis.Axis_Z)
                {
                    alterVoxel(x, y, z + direction, state);
                }
            }

            ClrVoxel(x, y, z, 7);
        }
    }
}
