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
    /// Logique d'interaction pour Function.xaml
    /// </summary>
    public partial class Function : UserControl
    {
        private Cube _cube3D;
        private CubeFCT _cubeFonction;
        public Function(Cube cube)
        {
            _cube3D = cube;
            _cubeFonction = new CubeFCT(_cube3D);
            InitializeComponent();
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            GroupBox_Cursor.Visibility = System.Windows.Visibility.Collapsed;

            if (ComboBox.SelectedItem == Combo_FeuArtifice)
                _cube3D.StartProcess(_cubeFonction.FeuArtifice);
            else if (ComboBox.SelectedItem == Combo_ShiftWall)
                _cube3D.StartProcess(_cubeFonction.ShiftWall);
            else if (ComboBox.SelectedItem == Combo_RotationSinus)
                _cube3D.StartProcess(_cubeFonction.SinusRotation);
            else if (ComboBox.SelectedItem == Combo_FaceTombante)
                _cube3D.StartProcess(_cubeFonction.FaceTombante);
            else if(ComboBox.SelectedItem == Combo_Math3D)
                _cube3D.StartProcess(_cubeFonction.Math3D);
            else if (ComboBox.SelectedItem == Combo_EffetRain)
                _cube3D.StartProcess(_cubeFonction.EffectRain);
            else if (ComboBox.SelectedItem == Combo_EffetSlide)
                _cube3D.StartProcess(_cubeFonction.EffectSlide);
            else if (ComboBox.SelectedItem == Combo_EffetBox)
                _cube3D.StartProcess(_cubeFonction.EffectBox);
            else if (ComboBox.SelectedItem == Combo_JeuJohnConway)
                _cube3D.StartProcess(_cubeFonction.JeuJohnConway);
            else if (ComboBox.SelectedItem == Combo_Snake)
            {
                _cube3D.StartProcess(_cubeFonction.Snake);
                GroupBox_Cursor.Visibility = System.Windows.Visibility.Visible;
            }
            else if (ComboBox.SelectedItem == Combo_SnakeRandom)
                _cube3D.StartProcess(_cubeFonction.SnakeRandom);
            else if (ComboBox.SelectedItem == Martin)
                _cube3D.StartProcess(_cubeFonction.martin);

            Thread.Sleep(50);
            RefreshWindow();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _cube3D.CloseProcess();
        }

        private void Slider_Temps_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_cube3D != null)
            {
                _cube3D.TimeFCT = (int)Slider_Temps.Value;
                if (TexBlock_ValueSliderTemps != null)
                    TexBlock_ValueSliderTemps.Text = _cube3D.TimeFCT.ToString();

            }

        }

        private void Slider_RGB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_cube3D != null)
            {
                _cube3D.RGB = (int)Slider_RGB.Value;
                if (TexBlock_ValueSliderRGB != null)
                    TexBlock_ValueSliderRGB.Text = _cube3D.RGB.ToString();

            }
        }

        public void RefreshWindow()
        {
            Slider_Temps.Value = _cube3D.TimeFCT;
            Slider_RGB.Value = _cube3D.RGB;
            TexBlock_ValueSliderTemps.Text = _cube3D.TimeFCT.ToString();
            TexBlock_ValueSliderRGB.Text = _cube3D.RGB.ToString();
        }

        private void TextBlock_Cursor_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(TextBlock_Cursor);
            _cubeFonction.x = ((int)p.X) / 20;
            _cubeFonction.y = ((int)p.Y) / 20;
        }       
    }
}
