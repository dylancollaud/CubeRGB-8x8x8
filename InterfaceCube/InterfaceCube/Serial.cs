using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterfaceCube
{
    class Serial
    {
        public int TimeWaitReconnect { get; set; }

        public Thread ThreadConnect { get; set; }
        public SerialPort SerialPort { get; set; }
        private string dataReceive;
        public bool BoucleActive { get; set; }

        public int NumCom { get; set; }

        public Serial()
        {
            TimeWaitReconnect = 1000;
            NumCom = 7;
            SerialPort = new SerialPort();
            BoucleActive = true;
        }

        // Démarre le thread de laison 
        public void StartConnect()
        {
            if (ThreadConnect != null && ThreadConnect.IsAlive)
                ThreadConnect.Abort();

            ThreadConnect = new Thread(new ThreadStart(Connect)) { IsBackground = true };
            ThreadConnect.Start();
        }

        public bool IsConnected()
        {
            return !BoucleActive;
        }

        // Tâche de connexion au Serial
        // Tourne en boucle tant qu'il n'est pas connecté
        public void Connect()
        {
            BoucleActive = true;
            

            SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            while (BoucleActive)
            {
                if(!SerialPort.IsOpen){

                    SerialPort.BaudRate = 57600;
                    SerialPort.PortName = "COM"+NumCom;
                     
                    try
                    {
                        SerialPort.Open();
                        if (SerialPort.IsOpen)
                        {
                            Console.WriteLine("Connected !");
                            BoucleActive = false;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Unable to connect to Arduino !");
                        Thread.Sleep(TimeWaitReconnect);
                        BoucleActive = true;
                    }
                }else
                {
                    BoucleActive = false;
                }
            }

        }
        
        // Déconnect la connexion Serial
        public void Disconnect()
        {
            if (SerialPort.IsOpen)
            {
                try
                {
                    SerialPort.Close();
                }catch (Exception)
                {
                    Console.WriteLine("Unable to close Serial !");
                }
            }
            else
            {
                BoucleActive = false;
            }
            SerialPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);

        }

        // Evenement de reception d'un message Serial
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            dataReceive += sp.ReadExisting();
            
                Console.WriteLine(dataReceive);
                dataReceive = "";
            
        }

        public bool Write(byte[] buf, int off, int count)
        {
            try
            {
                SerialPort.Write(buf, off, count);
                return true;
            }catch(Exception)
            {
                
                return false;
            }
        }
    }
}
