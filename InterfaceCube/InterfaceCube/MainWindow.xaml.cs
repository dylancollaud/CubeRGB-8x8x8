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
    public partial class MainWindow : Window
    {
        public Cube Cube3D;


        public Function userFunction;
        public Test userTest;
        public Configuration userConfiguration;
        public Sound userSound;

        public MainWindow()
        {
            InitializeComponent();
            Thread.Sleep(500);
            Cube3D = new Cube(this);
            userFunction = new Function(Cube3D);
            userTest = new Test(Cube3D);
            userConfiguration = new Configuration(Cube3D);
            userSound = new Sound();

            Function.Content = userFunction;
            Test.Content = userTest;
            Configuration.Content = userConfiguration;
            Sound.Content = userSound;


            Button_ConnectSerial.IsEnabled = true;

            Cube3D.ConnectSerial(22);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cube3D.Close();
        }

        private void Menu_Close_Click(object sender, RoutedEventArgs e)
        {
            Cube3D.Close();

            Close();
        }

        private void Button_ResetCube_Click(object sender, RoutedEventArgs e)
        {
            Cube3D.CloseProcess();
            Cube3D.ResetCube();
        }

        private void Button_StartSerial_Click(object sender, RoutedEventArgs e)
        {
            int com;
            if (int.TryParse(userConfiguration.TextBox_Com.Text, out com))
            {
                Cube3D.ConnectSerial(com);
            }else
            {
                MessageBox.Show("Port com isn't a number !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
