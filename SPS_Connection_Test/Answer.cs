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

        public Answer(Request request, NetworkStream n)
        {
            this.request = request;
            this.n = n;

            //Sendet Glühungen
            if (request.All.Contains("getAnnealing"))
            {

                var glu = (from g in d.Glühung
                           where g.Id_Ofen == request.OId
                           select g).Take(request.Anzahl);

                foreach (var i in glu)
                {
                    SendAnswer(n, request.Id + i.Id_Intern + " " + i.Name + " " + i.Tonnage + "kg");
                }
            }
            //Glühung verschieben mit GlühungsId und OfenId
            if (request.All.Contains("moveAnnealing"))
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

            if (request.All.Contains("getOrdersInAnnealing"))
            {
                var ord = (from o in d.Auftrag
                           join m in d.Material on o.Id equals m.Id_Auftrag
                           join g in d.Glühung on m.Id_Glühung equals g.Id
                           where g.Id == request.GId
                           select new { o.ODL }).Take(request.Anzahl);

                foreach (var a in ord)
                {
                    SendAnswer(n, request.Id + a.ODL + "Abmessung???" + "rd" + "Tonnage in der Glühung" + "kg" + "Stückzahl in der Glühung" + "/" + "Gesamtstückzahl" + "kg");
                }

            }
            if (request.All.Contains("getOrderEqAnnealing"))
            {
                var ord = (from o in d.Auftrag
                           join m in d.Material on o.Id equals m.Id_Auftrag
                           join g in d.Glühung on m.Id_Glühung equals g.Id
                           where g.Id == request.GId
                           select new { o.ODL }).Take(request.Anzahl);

                foreach (var a in ord)
                {
                    SendAnswer(n, request.Id + a.ODL + "Abmessung???" + "rd" + "Tonnage in der Glühung" + "kg" + "Stückzahl in der Glühung" + "/" + "Gesamtstückzahl" + "kg");
                }
            }
            if (request.All.Contains("getItemsInAnnealing"))
            {
                var mat = from m in d.Material
                          where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                          select m;

                foreach (var i in mat)
                {
                    SendAnswer(n, request.Id + Convert.ToString(i.Id) + i.Gewicht + "kg");
                }
            }
            if (request.All.Contains("getItemsOutAnnealing"))
            {
                var mat = from m in d.Material
                          where m.Id_Glühung == request.GId
                          select m;

                foreach (var i in mat)
                {
                    SendAnswer(n, request.Id + Convert.ToString(i.Id) + i.Gewicht + "kg");
                }
            }
            if (request.All.Contains("moveItemToReserve"))
            {
                var mat = from g in d.Material
                          where g.Id == request.IId
                          select g;

                Material l = mat.First();
                l.Id_Glühung = 0;

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                }
            }
            if (request.All.Contains("moveItemToAnnealing"))
            {
                var mat = from g in d.Material
                          where g.Id == request.IId
                          select g;

                Material l = mat.First();
                l.Id_Glühung = request.GId;

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                }

            }
            if (request.All.Contains("moveAllItemsToReserve"))
            {
                var mat = from m in d.Material
                          where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                          select m;

                foreach (Material i in mat)
                {
                    i.Id_Glühung = 0;
                }

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                }
            }
            if (request.All.Contains("moveAllItemsToAnnealing"))
            {
                var mat = from m in d.Material
                          where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                          select m;

                foreach (Material i in mat)
                {
                    i.Id_Glühung = request.GId;
                }

                try
                {
                    d.SubmitChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                }
            }
            if (request.All.Contains("setAnnealingStat"))
            {
                var glu = from g in d.Glühung
                          where g.Id == request.GId
                          select g;

                switch (request.Status)
                {
                    //Wird bearbeitet
                    case 1:
                        glu.First().Status = "Wird verarbeitet!";
                        break;
                    //Ist beendet
                    case 2:
                        glu.First().Status = "Ist beendet!";
                        break;
                    //Glühfreigabe aufgehoben
                    case 3:
                        glu.First().Status = "Freigabe aufgehoben!";
                        break;
                    default:
                        Console.WriteLine("Fehler! Annealingsstatus nicht erkannt!");
                        break;
                }

            }
        }
        public void SendAnswer(NetworkStream n, string s)
        {
            Paket p = new Paket(s);
            p.Verpacke();

            if (n.CanWrite)
            {
                n.Write(p.getData(), 0, p.getLenght());

                Console.WriteLine("Nachricht verschickt: \t " + Encoding.ASCII.GetString(p.getData()));
            }
            else
            {
                Console.WriteLine("Netzwerkstream wurde abgebrochen! Es kann nicht mehr geschrieben werden!");
            }


        }
    }
}
