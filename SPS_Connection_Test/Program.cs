using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPS_Connection_Test
{
    class Program
    {

        static String s_message = "Hallo! Ich bin eine Nachricht!";
        //static String r_message;


        static void Main(string[] args)
        {
            Start_t();
            


            Console.ReadKey();

        }

        static void Listen()
        {
            //10.222.47.122
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 5000);

            tcpListener.Start();
            

            Console.WriteLine("Sleep...");
            Thread.Sleep(2000);
            Console.WriteLine("Start thread...");


            Console.WriteLine("Warte auf Verbindung...\n");

            TcpClient tcpClient = tcpListener.AcceptTcpClient();
            Socket s = tcpClient.Client;

            try
            {
                Console.WriteLine("Verbunden! mit: " + IPAddress.Parse(((IPEndPoint)s.RemoteEndPoint).Address.ToString()) + "\n ");
            }
            catch (Exception e)
            {
                Console.WriteLine("IP konnte nicht ermittelt werden! Ungültiger Client!\n" + e);
            }

            while (true)
            {
                try
                {
                    NetworkStream nStream = tcpClient.GetStream();
                    
                    Cycle(nStream, tcpClient);


                    tcpClient.Close();
                    Console.WriteLine("Verbindung getrennt!");
                }
                catch (Exception e)
                {
                    tcpClient.Close();

                    Console.WriteLine("Übertragung fehlerhaft!\n"); //+ e.ToString());

                    tcpListener.Stop();
                    tcpListener = null;
                    Listen();

                }

            }


        }

        static void Start_t()
        {

            Thread t = new Thread(Listen);

                t.Start();

        }

        static void Cycle(NetworkStream n, TcpClient t)
        {

            byte[] myReadBuffer = new byte[1024 * 8];
            string m = String.Empty;

            if (n.CanRead)
            {
                Console.WriteLine("Lese...\n");

                int numberOfBytesRead = 0;

                while (t.Connected)
                {
                    numberOfBytesRead = n.Read(myReadBuffer, 0, myReadBuffer.Length);

                    if (numberOfBytesRead > 0)
                    {
                        Paket e = new Paket(myReadBuffer);

                        Request request = new Request(e.Daten, n);
                    }
                }

            }
            else
            {
                Console.WriteLine("Netzwerkstream wurde abgebrochen! Es kann nicht mehr gelesen werden!");
            }
        }
    }
}
