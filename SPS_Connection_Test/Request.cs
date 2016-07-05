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
        //Für die Auftragsverwaltung
        Byte[] allBytes; //Ganzer Befehl
        int gId;         //GlühungsId
        int oId;         //OfenId
        int aId;         //AuftragsId
        int iId;         //ItemId
        int anzahl;      //Anzahl
        int status;      //Status
        string befehl;   //String für den Befehl                                    
        //Für die Prozessverwaltung
        int ebene;       //Ebene des Belegungsplans
        int position;    //Position des Feldes auf dem Belegungsplan
        int stückId;     //Id des Items

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
        public int Ebene
        {
            get
            {
                return ebene;
            }

            set
            {
                ebene = value;
            }
        }
        public int Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public int StückId
        {
            get
            {
                return stückId;
            }

            set
            {
                stückId = value;
            }
        }

        //Auftragsverwaltung
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

        //Allgemeine Befehle
        static string STRING_FEHLER_E_HEAD = "E_HEAD";
        static string STRING_FEHLER_TOLONG = "TOLONG";
        static string STRING_FEHLER_NALIGN = "NALIGN";

        static string STRING_CRC_OK = "CRC_OK";
        static string STRING_CRC_NOK = "CRCNOK";

        //Prozessverwaltung
        public static string STRING_GET_ASSIGNMENT = "getAssignment";
        public static string STRING_SET_ASSIGNMENT = "setAssignment";

        public static string STRING_SAVE_LOG_15S = "saveLog15s";
        public static string STRING_SAVE_LOG_30S = "saveLog30s";
        public static string STRING_SAVE_LOG_60S = "saveLog60s";

        public static string STRING_GET_PROGRAM = "getProgram";

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



            if (a.Contains(STRING_FEHLER_E_HEAD))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + STRING_FEHLER_E_HEAD +"\n");
            }
            else if (a.Contains(STRING_FEHLER_E_HEAD))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + STRING_FEHLER_E_HEAD + "\n");
            }
            else if (a.Contains(STRING_FEHLER_TOLONG))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + STRING_FEHLER_TOLONG + "\n");
            }
            else if (a.Contains(STRING_FEHLER_NALIGN))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + STRING_FEHLER_NALIGN + "\n");
            }
            else if (a.Contains(STRING_CRC_OK))
            {
                Console.WriteLine("Erfolgreich versendete Nachricht!: " + STRING_CRC_OK + "\n");
            }
            else if (a.Contains(STRING_CRC_NOK))
            {
                Console.WriteLine("Fehler bei Nachricht!: " + STRING_CRC_NOK + "\n");
            }
            else
            {
                if (allBytes.Length > 34)
                {
                    byte[] tmp = allBytes.Skip(34).ToArray();

                    Request_Auftrag(a, tmp);
                    Request_Prozess(a, tmp);
                }
            }


            if (Befehl != String.Empty)
            {
                Console.WriteLine("Befehl: " +  Befehl + "\n" + " ID: " + Id + " OID: " + OId + " Anzahl: " + Anzahl  +
                    " GID: " + GId + " AID: "+ AId + " IID: " + IId + " Status: " + Status + "\n " +
                    "Ebene: " + Ebene + " Position: " + Position + " StückId: " + StückId + "\n ");

                Answer aw = new Answer(this);
            }
        }

        private void Request_Prozess(string a, byte[] tmp)
        {
            if (a.Contains(STRING_GET_ASSIGNMENT))
            {
                try
                {
                    Befehl = STRING_GET_ASSIGNMENT;
                    GId = Convert.ToInt32(tmp.Take(4).Reverse().ToArray());
                    Ebene = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0));
                }
                catch (Exception)
                {
                    Console.WriteLine("Befehl: " + STRING_GET_ASSIGNMENT + " konnte nicht ausgeführt werden!");
                }
            }
            if (a.Contains(STRING_SET_ASSIGNMENT))
            {
                try
                {
                    Befehl = STRING_SET_ASSIGNMENT;
                    StückId = Convert.ToInt32(tmp.Take(4).Reverse().ToArray());
                    Position = Convert.ToInt32(BitConverter.ToInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0));
                }
                catch (Exception)
                {
                    Console.WriteLine("Befehl: " + STRING_SET_ASSIGNMENT + " konnte nicht ausgeführt werden!");
                }
            }
            if (a.Contains(STRING_SAVE_LOG_15S))
            {
                try
                {
                    Befehl = STRING_SAVE_LOG_15S;
                }
                catch (Exception)
                {
                    Console.WriteLine("Befehl: " + STRING_SAVE_LOG_15S + " konnte nicht ausgeführt werden!");
                }
            }
            if (a.Contains(STRING_SAVE_LOG_30S))
            {
                try
                {
                    Befehl = STRING_SAVE_LOG_30S;
                }
                catch (Exception)
                {
                    Console.WriteLine("Befehl: " + STRING_SAVE_LOG_30S + " konnte nicht ausgeführt werden!");
                }
            }
                if (a.Contains(STRING_SAVE_LOG_60S))
                {
                    try
                    {
                        Befehl = STRING_SAVE_LOG_60S;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Befehl: " + STRING_SAVE_LOG_60S + " konnte nicht ausgeführt werden!");
                    }
                }
            if (a.Contains(STRING_GET_PROGRAM))
            {
                try
                {
                    Befehl = STRING_GET_PROGRAM;
                    GId = Convert.ToInt32(BitConverter.ToInt16(tmp.Take(4).Reverse().ToArray(), 0));
                }
                catch (Exception)
                {
                    Console.WriteLine("Befehl: " + STRING_GET_PROGRAM + " konnte nicht ausgeführt werden!");
                }
            }
        }

        private void Request_Auftrag(string a, byte[] tmp)
        {
            //if (allBytes.Length > 34)
            //{
                //byte[] tmp = allBytes.Skip(34).ToArray();

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

            //}
        }
    }

}
