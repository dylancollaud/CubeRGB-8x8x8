using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace InterfaceCube
{
    /// <summary>
    /// Logique d'interaction pour Sound.xaml
    /// </summary>
    public partial class Sound : UserControl
    {
        private WaveIn waveIn;
        private int Fs = 48000;
        public Sound()
        {
            InitializeComponent();
            
        }

        public void record()
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(Fs, 1); 
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            
                //Debug.WriteLine("Flushing Data Available");
                Console.Write(e.Buffer.ToString());
                waveIn.StopRecording();
           
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            record();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }
    }
}
