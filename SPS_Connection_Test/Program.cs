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
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 5000);

            try
            {
                tcpListener.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("Verbindungsprobleme...IP nicht erreichbar!");
            }


            Paket p = new Paket(Encoding.ASCII.GetBytes(s_message));
            p.Verpacke();

            try
            {

                while (true)
                {
                    Console.WriteLine("Sleep...");
                    Thread.Sleep(2000);
                    Console.WriteLine("Start thread...");


                    Console.WriteLine("Warte auf Verbindung...");

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Console.WriteLine("Verbunden!");

                    NetworkStream nStream = tcpClient.GetStream();

                    //Send(nStream, p);

                    Cycle(nStream, tcpClient);


                    tcpClient.Close();
                    Console.WriteLine("Verbindung getrennt!");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry.  You cannot read from this NetworkStream....");

            }
            finally
            {
                tcpListener.Stop();
          
                Start_t();

                
            }
        }

        static void Start_t() {

            Thread t = new Thread(Listen);
     
            t.Start();
        }
        
        static void Cycle(NetworkStream n, TcpClient t ) {

            byte[] myReadBuffer = new byte[1024 * 8];
            string m = String.Empty;

            if (n.CanRead)
            {
                Console.WriteLine("Lese...");
                //StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;

                while (t.Connected)
                {
                    numberOfBytesRead = n.Read(myReadBuffer, 0, myReadBuffer.Length);

                    if (numberOfBytesRead > 0)
                    {
                        Paket e = new Paket(myReadBuffer);
                        m = e.Entpacke();

                        Console.WriteLine("Empfangen: " + m);

                        Request request = new Request(m);
                        Answer answer = new Answer(request,n);
                        //request.GenerateAnswer();
                        //request.SendAnswer(n);
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Netzwerkstream wurde abgebrochen! Es kann nicht mehr gelesen werden!"); 
            }
           // return m;
        }
    }
}

//Paket e = new Paket(myReadBuffer);
//e.Entpacke();

//StringBuilder myCompleteMessage = null;
//myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
//Console.WriteLine("Empfangene Nachricht : " +
//                             myCompleteMessage);
//  m = m + myCompleteMessage;
//myCompleteMessage.Clear();