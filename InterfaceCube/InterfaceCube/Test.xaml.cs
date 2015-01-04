using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfaceCube
{
    /// <summary>
    /// Logique d'interaction pour Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        private Cube _cube3D;
        private object _lastSender;
        public Test(Cube cube3D)
        {
            _cube3D = cube3D;
            InitializeComponent();
        }

        private void refreshCube(object sender)
        {
            if (!_cube3D.IsStarted())
            {
                _cube3D.StartProcess(_cube3D.Process);
                Thread.Sleep(1000);
            }

            _cube3D.Fill(0, 0);

            if (_cube3D != null && Slider_RGB != null)
            {
                if ((bool)RadioButton_Pixel.IsChecked)
                    _cube3D.SetVoxel((int)Slider_X.Value, (int)Slider_Y.Value, (int)Slider_Z.Value, (int)Slider_RGB.Value);
                else
                {
                    if (sender != Slider_RGB)
                    {
                        _lastSender = sender;
                    }else
                    {
                        sender = _lastSender;
                    }

                    if (sender == Slider_X)
                    {
                        _cube3D.SetPlane_x((int)Slider_X.Value, (int)Slider_RGB.Value);
                    }
                    else if (sender == Slider_Y)
                    {
                        _cube3D.SetPlane_y((int)Slider_Y.Value, (int)Slider_RGB.Value);
                    }
                    else if (sender == Slider_Z)
                    {
                        _cube3D.SetPlane_z((int)Slider_Z.Value, (int)Slider_RGB.Value);
                    }                 
                }
            }

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            refreshCube(sender);
        }

        private void Slider_Cube_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_cube3D.IsStarted())
            {
                _cube3D.StartProcess(_cube3D.Process);
                Thread.Sleep(1000);
            }
            _cube3D.Fill(0, 0);
            if (Slider_Cube != null)
                _cube3D.Fill(255, (int)Slider_Cube.Value);
        }



    }
}
