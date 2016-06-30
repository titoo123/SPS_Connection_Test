using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SPS_Connection_Test
{
    public class Answer
    {
        LINQDataContext d = new LINQDataContext();

        Request request;
        NetworkStream n;

        public Answer(Request request)
        {
            this.request = request;
            this.n = request.N;

            //Sendet Glühungen
            if (request.Befehl.Contains(Request.STRING_GET_ANNEALING))
            {
                //var glu = (from g in d.Glühung
                //           where g.Id_Ofen == request.OId
                //           select g).Take(request.Anzahl);

                //foreach (var i in glu)
                //{
                    SendAnswer(
                              n,BitConverter.GetBytes(Convert.ToInt16(request.Id))
                        .Concat(BitConverter.GetBytes(request.AId))
                        .Concat(BitConverter.GetBytes(1))
                        .Concat(Encoding.ASCII.GetBytes(FixStringLenght("001/2016 800C/10h 34567Kg")))
                        .ToArray());
                //}
            }
            //Glühung verschieben mit GlühungsId und OfenId
            if (request.Befehl.Contains(Request.STRING_MOVE_ANNEALING))
            {
                var glu = from g in d.Glühung
                          where g.Id == request.GId
                          select g;

                Glühung l = glu.First();
                l.Id_Ofen = request.OId;

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                }
            }

            if (request.Befehl.Contains(Request.STRING_GET_ORDERS_IN_ANNEALING))
            {
                //var ord = (from o in d.Auftrag
                //           join m in d.Material on o.Id equals m.Id_Auftrag
                //           join g in d.Glühung on m.Id_Glühung equals g.Id
                //           where g.Id == request.GId
                //           select new { o.ODL }).Take(request.Anzahl);

                //foreach (var a in ord)
                //{
                //    // SendAnswer(n, request.Id + a.ODL + "Abmessung???" + "rd" + "Tonnage in der Glühung" + "kg" + "Stückzahl in der Glühung" + "/" + "Gesamtstückzahl" + "kg");
                //}
                SendAnswer(
                          n, BitConverter.GetBytes(Convert.ToInt16(request.Id))
                    .Concat(BitConverter.GetBytes(request.GId))
                    .Concat(BitConverter.GetBytes(1))
                    .Concat(Encoding.ASCII.GetBytes( FixStringLenght("001 456 22rd 5654Kg 2/10")))
                    .ToArray());

            }
            if (request.Befehl.Contains(Request.STRING_GET_ORDERS_EQ_ANNEALING))
            {
                //var ord = (from o in d.Auftrag
                //           join m in d.Material on o.Id equals m.Id_Auftrag
                //           join g in d.Glühung on m.Id_Glühung equals g.Id
                //           where g.Id == request.GId
                //           select new { o.ODL }).Take(request.Anzahl);

                //foreach (var a in ord)
                //{
                //    //  SendAnswer(n, request.Id + a.ODL + "Abmessung???" + "rd" + "Tonnage in der Glühung" + "kg" + "Stückzahl in der Glühung" + "/" + "Gesamtstückzahl" + "kg");
                //}
                SendAnswer(
                        n, BitConverter.GetBytes(Convert.ToInt16(request.Id))
                .Concat(BitConverter.GetBytes(request.GId))
                .Concat(BitConverter.GetBytes(1))
                .Concat(Encoding.ASCII.GetBytes(FixStringLenght("021  56 22rd 5654Kg 2/10")))
                .ToArray());
            }
            if (request.Befehl.Contains(Request.STRING_GET_ITEMS_IN_ANNEALING))
            {
                //var mat = from m in d.Material
                //          where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                //          select m;

                //foreach (var i in mat)
                //{
                //    // SendAnswer(n, request.Id + Convert.ToString(i.Id) + i.Gewicht + "kg");
                //}
            }
            if (request.Befehl.Contains(Request.STRING_GET_ITEMS_OUT_ANNEALING))
            {
                //var mat = from m in d.Material
                //          where m.Id_Glühung == request.GId
                //          select m;

                //foreach (var i in mat)
                //{
                //    //  SendAnswer(n, request.Id + Convert.ToString(i.Id) + i.Gewicht + "kg");
                //}
            }
            if (request.Befehl.Contains(Request.STRING_MOVE_ALL_ITEMS_TO_RESERVE))
            {
                //var mat = from g in d.Material
                //          where g.Id == request.IId
                //          select g;

                //Material l = mat.First();
                //l.Id_Glühung = 0;

                //try
                //{
                //    d.SubmitChanges();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                //}
            }
            if (request.Befehl.Contains(Request.STRING_MOVE_ITEM_TO_ANNEALING))
            {
                //var mat = from g in d.Material
                //          where g.Id == request.IId
                //          select g;

                //Material l = mat.First();
                //l.Id_Glühung = request.GId;

                //try
                //{
                //    d.SubmitChanges();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                //}

            }
            if (request.Befehl.Contains(Request.STRING_MOVE_ALL_ITEMS_TO_RESERVE))
            {
                //var mat = from m in d.Material
                //          where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                //          select m;

                //foreach (Material i in mat)
                //{
                //    i.Id_Glühung = 0;
                //}

                //try
                //{
                //    d.SubmitChanges();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                //}
            }
            if (request.Befehl.Contains("moveAllItemsToAnnealing"))
            {
                //var mat = from m in d.Material
                //          where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                //          select m;

                //foreach (Material i in mat)
                //{
                //    i.Id_Glühung = request.GId;
                //}

                //try
                //{
                //    d.SubmitChanges();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                //}
            }
            if (request.Befehl.Contains("setAnnealingStat"))
            {
                //var glu = from g in d.Glühung
                //          where g.Id == request.GId
                //          select g;

                //switch (request.Status)
                //{
                //    //Wird bearbeitet
                //    case 1:
                //        glu.First().Status = "Wird verarbeitet!";
                //        break;
                //    //Ist beendet
                //    case 2:
                //        glu.First().Status = "Ist beendet!";
                //        break;
                //    //Glühfreigabe aufgehoben
                //    case 3:
                //        glu.First().Status = "Freigabe aufgehoben!";
                //        break;
                //    default:
                //        Console.WriteLine("Fehler! Annealingsstatus nicht erkannt!");
                //        break;
                //}

            }
        }
        public void SendAnswer(NetworkStream n, byte[] s)
        {
            Paket p = new Paket(s);
            p.Verpacke();

            if (n.CanWrite)
            {
                n.Write(p.getData(), 0, p.getLenght());

                Console.WriteLine("Nachricht verschickt: \t " + Encoding.ASCII.GetString(p.getData()) + "\n");
            }
            else
            {
                Console.WriteLine("Netzwerkstream wurde abgebrochen! Es kann nicht mehr geschrieben werden!");
            }


        }
        public string FixStringLenght(string s) {
            if (s.Length > 30)
            {
                s = s.Substring(0, s.Length - 2);
                return FixStringLenght(s);
            }
            else if (s.Length < 30)
            {
                s = s + " ";
                return FixStringLenght(s);
            }
            else
            {
                return s;
            }
        }
    }
}
