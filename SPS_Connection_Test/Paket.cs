using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;



namespace SPS_Connection_Test
{

    class Paket
    {
        // private const UInt32 startwert = 3203383023;
        byte[] startwert = { 190, 239, 190, 239 };
        //Bit[] startwert =  { 1,0,1,1,1,1,1,0,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,0,1,1,1,1 };
        CRC tc = new CRC();

        //Int32 prüfsumme;
        Int16 nutzdatenlänge;
        Int16 gesamtlänge;

        Byte[] byteArray;
        Byte[] daten;

        public byte[] Daten
        {
            get
            {
                return daten;
            }

        }

        public Paket(Byte[] daten)
        {
            //this.daten = daten.Skip(4).ToArray(); 
            this.daten = daten;
        }
        //public Paket(string s)
        //{
        //    this.daten = Encoding.ASCII.GetBytes(s);
        //}

        public void Verpacke()
        {

            //Fügt Stratwert hinzu
            byteArray = startwert;

            //Übergibt Hashwert
            tc.AddData(Daten);
            byteArray = byteArray.Concat(BitConverter.GetBytes(tc.Crc32Value).Reverse()).ToArray();

            //Übergibt Nutzerdatenlänge
            nutzdatenlänge = Convert.ToInt16(Daten.Length);
            byte[] nL = BitConverter.GetBytes(nutzdatenlänge).Reverse().ToArray();
            byteArray = byteArray.Concat(nL).ToArray();
            //byteArray = byteArray.Concat(BitConverter.GetBytes(nutzdatenlänge)).ToArray();

            //Übergibt Gesamtlänge
            gesamtlänge = getGesamtlänge(byteArray.Length + nutzdatenlänge + 2);
            byte[] gL = BitConverter.GetBytes(gesamtlänge).Reverse().ToArray();
            byteArray = byteArray.Concat(gL).ToArray();
            //byteArray = byteArray.Concat(BitConverter.GetBytes(gesamtlänge)).ToArray();

            //Übergibt Nutzerdaten
            byteArray = byteArray.Concat(Daten).ToArray();

            //Prepariert Daten für transport
            byteArray = Prepare(byteArray);
        }
        //public string Entpacke()
        //{

        //    if (Daten.Length > 0)
        //    {
        //        if (TrueMessage(Daten.Take(4)))
        //        {
        //            //if (TrueValue(Daten.Skip(4).Take(4)))
        //            //{
        //                nutzdatenlänge = BitConverter.ToInt16(Daten.Skip(8).Take(2).Reverse().ToArray(), 0);
        //                gesamtlänge = BitConverter.ToInt16(Daten.Skip(10).Take(2).Reverse().ToArray(), 0);
        //                byteArray = Daten.Skip(12).Take(nutzdatenlänge).ToArray();

        //            return Encoding.ASCII.GetString(Daten, 0, Daten.Length);
        //            //return Encoding.ASCII.GetString(byteArray, 0, nutzdatenlänge);
        //            //}

        //        }
        //    }

        //    return "";

        //}

        private bool TrueValue(IEnumerable<byte> enumerable)
        {
            tc.AddData(Daten.Skip(2).Take(14).ToArray());
            //if (enumerable.SequenceEqual( BitConverter.GetBytes(tc.Crc32Value)))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        private bool TrueMessage(IEnumerable<byte> enumerable)
        {
            if (startwert.SequenceEqual(enumerable))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool TrueMessage() {

            return true;
        }
        public Int16 getGesamtlänge(Int32 n)
        {
            Int32 l = n;
            while (l % 20 != 0)
            {
                l++;
            }
            int d = l - n;

            return Convert.ToInt16(d + nutzdatenlänge);
        }

        public Byte[] Prepare(Byte[] m)
        {
            Byte[] endByteArray = new Byte[20];

            //Füllt EndArray
            for (int i = 0; i < 20; i++)
            {
                endByteArray[i] = 255;
            }

            while (m.Length % 20 != 0)
            {
                m = m.Concat(new Byte[] { 255 }).ToArray();
            }

            return m.Concat(endByteArray).ToArray();
        }

        public Byte[] getData()
        {

            return this.byteArray;
        }
        public String getDataString()
        {
            return BitConverter.ToString(byteArray);
            //return System.Text.Encoding.UTF8.GetString(this.byteArray);
        }


        public string ByteArrayToString()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            foreach (byte b in this.byteArray)
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            return sb.ToString();
        }
        public int getLenght()
        {
            return byteArray.Length;
        }


    }
}
