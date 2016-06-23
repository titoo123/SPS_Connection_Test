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

        //Werte zum Datenversand
        string id;      //ID der Anfrage

        //Werte zum Datenempfang
        Byte[] allBytes;//Ganzer Befehl
        string all;     //Ganzer Befehl "string"
        int gId;        //GlühungsId
        int oId;        //OfenId
        int aId;        //AuftragsId
        int iId;        //ItemId
        int anzahl;     //Anzahl
        int status;     //Status

        //
        
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
        public string Id
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
        public string All
        {
            get
            {
                return all;
            }

            set
            {
                all = value;
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


        //Übernimmt gesamte Anfrage und teilt diese
        public Request(string a)
        {
            this.All = a;            
            //Übergibt/Konvertiert alle Bytes
            allBytes = Encoding.ASCII.GetBytes(a);
            //Extrahiert die ID
            id = Convert.ToString( Convert.ToInt16(allBytes.Take(2)));
            string[] r = { };

            if (a.Contains("getAnnealing"))
            {
                r = a.Split(new string[] { "getAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                OId = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Take(2)));
                Anzahl = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(2).Take(2)));

            }

            if (a.Contains("moveAnnealing"))
            {
                r = a.Split(new string[] { "moveAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                OId = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(4).Take(2)));
            }
            if (a.Contains("getOrdersInAnnealing"))
            {
                r = a.Split(new string[] { "getOrdersInAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                Anzahl = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(4).Take(2)));
            }
            if (a.Contains("getOrdersEqAnnealing"))
            {
                r = a.Split(new string[] { "getOrdersEqAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                Anzahl = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(4).Take(2)));
            }
            if (a.Contains("getItemsInAnnealing"))
            {
                r = a.Split(new string[] { "getItemsInAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                AId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Skip(4).Take(4));
                Anzahl = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(8).Take(2)));
            }
            if (a.Contains("getItemsOutAnnealing"))
            {
                r = a.Split(new string[] { "getItemsOutAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                AId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                Anzahl = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(4).Take(2)));
            }
            if (a.Contains("moveItemToReserve"))
            {
                r = a.Split(new string[] { "moveItemToReserve" }, StringSplitOptions.None);
                string t = r[1];

                IId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
            }
            if (a.Contains("moveItemToAnnealing"))
            {
                r = a.Split(new string[] { "moveItemToAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                IId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Skip(4).Take(4));
            }
            if (a.Contains("moveAllItemsToReserve"))
            {
                r = a.Split(new string[] { "moveAllItemsToReserve" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                AId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Skip(4).Take(4));
            }
            if (a.Contains("moveAllItemsToAnnealing"))
            {
                r = a.Split(new string[] { "moveAllItemsToAnnealing" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                AId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Skip(4).Take(4));
            }
            if (a.Contains("setAnnealingStat"))
            {
                r = a.Split(new string[] { "setAnnealingStat" }, StringSplitOptions.None);
                string t = r[1];

                GId = Convert.ToInt32(Encoding.ASCII.GetBytes(t).Take(4));
                Status = Convert.ToInt32(Convert.ToInt16(Encoding.ASCII.GetBytes(t).Skip(4).Take(2)));

            }

            Console.WriteLine("Nachrichtenteile:" +"\n" + r + "\n ");
        }
    }
}
