using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SPS_Connection_Test
{
    public class Request
    {
        //Zur Datenübertragung
        NetworkStream n;

        //Werte zum Datenversand
        int id;      //ID der Anfrage

        //Werte zum Datenempfang
        Byte[] allBytes; //Ganzer Befehl
        int gId;         //GlühungsId
        int oId;         //OfenId
        int aId;         //AuftragsId
        int iId;         //ItemId
        int anzahl;      //Anzahl
        int status;      //Status
        string befehl;   //String für den Befehl                                    


        public int Anzahl
        {
            get
            {
                return anzahl;
            }

            set
            {
                anzahl = value;
            }
        }
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
        public int GId
        {
            get
            {
                return gId;
            }

            set
            {
                gId = value;
            }
        }
        public int OId
        {
            get
            {
                return oId;
            }

            set
            {
                oId = value;
            }
        }
        public int AId
        {
            get
            {
                return aId;
            }

            set
            {
                aId = value;
            }
        }
        public int IId
        {
            get
            {
                return iId;
            }

            set
            {
                iId = value;
            }
        }
        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
        public NetworkStream N
        {
            get
            {
                return n;
            }

        }
        public string Befehl
        {
            get
            {
                return befehl;
            }
            set
            {
                befehl = value;
            }
        }

        public static string STRING_GET_ANNEALING = "getAnnealing";
        public static string STRING_MOVE_ANNEALING = "moveAnnealing";

        public static string STRING_GET_ORDERS_IN_ANNEALING = "getOrdersInAnnealing";
        public static string STRING_GET_ORDERS_EQ_ANNEALING = "getOrdersEqAnnealing";

        public static string STRING_GET_ITEMS_IN_ANNEALING = "getItemsInAnnealing";
        public static string STRING_GET_ITEMS_OUT_ANNEALING = "getItemsOutAnnealing";

        public static string STRING_MOVE_ITEM_TO_RESERVE = "moveItemToReserve";
        public static string STRING_MOVE_ITEM_TO_ANNEALING = "moveItemToAnnealing";

        public static string STRING_MOVE_ALL_ITEMS_TO_RESERVE = "moveAllItemsToReserve";
        public static string STRING_MOVE_ALL_ITEMS_TO_ANNEALING = "moveAllItemsToAnnealing";

        public static string STRING_SET_ANNEALING_STAT = "setAnnealingStat";

        static string FEHLER_E_HEAD = "E_HEAD";
        static string FEHLER_TOLONG = "TOLONG";
        static string FEHLER_NALIGN = "NALIGN";

        //Übernimmt gesamte Anfrage und teilt diese
        public Request(Byte[] allBytes, NetworkStream n)
        {
            this.n = n;
            //Übergibt/Konvertiert alle Bytes
            this.allBytes = allBytes;
            string a = Encoding.ASCII.GetString(allBytes);
            //Extrahiert die ID
            this.id = Convert.ToInt32(BitConverter.ToInt16(allBytes.Take(2).Reverse().ToArray(), 0));
            //this.allBytes = this.allBytes.Skip(4).ToArray();

            Befehl = String.Empty;



            if (a.Contains(FEHLER_E_HEAD))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + FEHLER_E_HEAD +"\n");
            }
            else if (a.Contains(FEHLER_E_HEAD))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + FEHLER_E_HEAD + "\n");
            }
            else if (a.Contains(FEHLER_TOLONG))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + FEHLER_TOLONG + "\n");
            }
            else if (a.Contains(FEHLER_NALIGN))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + FEHLER_NALIGN + "\n");
            }
            else
            {
                if (allBytes.Length > 34)
                {
                    byte[] tmp = allBytes.Skip(34).ToArray();

                    if (a.Contains(STRING_GET_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_GET_ANNEALING;
                            OId = Convert.ToInt32(BitConverter.ToInt16(tmp.Take(2).Reverse().ToArray(), 0));
                            Anzahl = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(2).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_GET_ANNEALING + "konnte nicht ausgeführt werden!");
                        }


                    }


                    else if (a.Contains(STRING_MOVE_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_MOVE_ANNEALING;
                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            OId = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_MOVE_ANNEALING + "konnte nicht ausgeführt werden!");
                        }
                    }
                    else if (a.Contains(STRING_GET_ORDERS_IN_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_GET_ORDERS_IN_ANNEALING;
                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            Anzahl = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_GET_ORDERS_IN_ANNEALING + "konnte nicht ausgeführt werden!");
                        }
                    }
                    else if (a.Contains(STRING_GET_ORDERS_EQ_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_GET_ORDERS_EQ_ANNEALING;
                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            Anzahl = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_GET_ORDERS_EQ_ANNEALING + "konnte nicht ausgeführt werden!");
                        }
                    }
                    else if (a.Contains(STRING_GET_ITEMS_IN_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_GET_ITEMS_IN_ANNEALING;

                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            AId = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(4).Reverse().ToArray(), 0));
                            Anzahl = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(8).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_GET_ITEMS_IN_ANNEALING + "konnte nicht ausgeführt werden!");
                        }
                    }
                    else if (a.Contains(STRING_GET_ITEMS_OUT_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_GET_ITEMS_OUT_ANNEALING;

                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            AId = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(4).Reverse().ToArray(), 0));
                            Anzahl = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(8).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_GET_ITEMS_OUT_ANNEALING + "konnte nicht ausgeführt werden!");
                        }

                    }
                    else if (a.Contains(STRING_MOVE_ITEM_TO_RESERVE))
                    {
                        try
                        {
                            Befehl = STRING_MOVE_ITEM_TO_RESERVE;

                            IId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_MOVE_ITEM_TO_RESERVE + "konnte nicht ausgeführt werden!");
                        }

                    }
                    else if (a.Contains(STRING_MOVE_ITEM_TO_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_MOVE_ITEM_TO_ANNEALING;

                            IId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            GId = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(4).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_MOVE_ITEM_TO_ANNEALING + "konnte nicht ausgeführt werden!");
                        }

                    }
                    else if (a.Contains(STRING_MOVE_ALL_ITEMS_TO_RESERVE))
                    {
                        try
                        {
                            Befehl = STRING_MOVE_ALL_ITEMS_TO_RESERVE;

                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            AId = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(4).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_MOVE_ALL_ITEMS_TO_RESERVE + "konnte nicht ausgeführt werden!");
                        }

                    }
                    else if (a.Contains(STRING_MOVE_ALL_ITEMS_TO_ANNEALING))
                    {
                        try
                        {
                            Befehl = STRING_MOVE_ALL_ITEMS_TO_ANNEALING;

                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            AId = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(4).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_MOVE_ALL_ITEMS_TO_ANNEALING + "konnte nicht ausgeführt werden!");
                        }

                    }
                    else if (a.Contains(STRING_SET_ANNEALING_STAT))
                    {
                        try
                        {
                            Befehl = STRING_SET_ANNEALING_STAT;

                            GId = BitConverter.ToInt32(tmp.Take(4).Reverse().ToArray(), 0);
                            Status = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Befehl: " + STRING_SET_ANNEALING_STAT + "konnte nicht ausgeführt werden!");
                        }

                    }

                    if (Befehl != String.Empty)
                    {
                        Console.WriteLine("Befehl: " + "\n" + Befehl + " ID: " + Id + " OID: " + OId + " Anzahl: " + Anzahl + " GID: " + GId + " AID: " + AId + " IID:" + IId + " Status: " + Status + "\n ");
                        Answer aw = new Answer(this);
                    }
                }
            }
        }

    }

}
