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

            Answer_Auftrag();
            Answer_Prozess();
        }
        public void SendAnswer(NetworkStream n, byte[] s)
        {
            Paket p = new Paket(s);
            p.Verpacke();

            if (n.CanWrite)
            {
                n.Write(p.getData(), 0, p.getLenght());

                Console.WriteLine("Nachricht verschickt:\n " + Encoding.ASCII.GetString(p.getData()) + "\n");
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

        private void Answer_Auftrag() {
            //Sendet Glühungen
            if (request.Befehl.Contains(Request.STRING_GET_ANNEALING))
            {

                var glu = (from g in d.Glühung
                               //where g.Id_Ofen == request.OId
                           select g).Take(request.Anzahl);

                if (glu.Count() > 0)
                {
                    byte[] ba = { };
                    foreach (var i in glu)
                    {
                        // string nr = String.Format("000", i.Id.ToString());
                        ba.Concat(BitConverter.GetBytes(i.Id));
                        ba.Concat(Encoding.ASCII.GetBytes(FixStringLenght(i.Name)));
                    }

                    SendAnswer(
                    n,
                    BitConverter.GetBytes(Convert.ToInt16(request.Id))
                    .Concat(ba)
                    .ToArray());
                }

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
                //Aufträge in Glühung
                var ord = (from o in d.Auftrag
                           join m in d.Material on o.Id equals m.Id_Auftrag
                           join g in d.Glühung on m.Id_Glühung equals g.Id
                           where g.Id == request.GId
                           select new { o.ODL, o.Abmessung1, o.Id, m.Id_Auftrag, m.Gewicht, m.Anzahl }
                           );
                // .Take(request.Anzahl);

                //Gesamtanzahl Material in Glühung
                var gaz = (from j in d.Material
                           where j.Id_Glühung == request.GId
                           select j.Anzahl).Sum();

                if (ord.Count() > 0)
                {
                    byte[] ba = { };
                    foreach (var a in ord)
                    {
                        //Gewicht pro Auftrag in Glühung
                        var gew = (from i in ord
                                   where i.Id == a.Id_Auftrag
                                   select i.Gewicht
                                   ).Sum();
                        var anz = (from l in ord
                                   where l.Id == a.Id_Auftrag
                                   select l.Anzahl
                                   ).Sum();

                        ba.Concat(Encoding.ASCII.GetBytes(FixStringLenght(a.ODL + " " + a.Abmessung1 + "rd " + gew.Value + "Kg " + anz.Value + "/" + gaz.Value)));
                    }

                    SendAnswer(
                    n, BitConverter.GetBytes(Convert.ToInt16(request.Id))
                    .Concat(BitConverter.GetBytes(request.AId))
                    .Concat(ba)
                    .ToArray());
                }

            }
            if (request.Befehl.Contains(Request.STRING_GET_ORDERS_EQ_ANNEALING))
            {
                //Aufträge in Glühung
                var ord = (from o in d.Auftrag
                           join m in d.Material on o.Id equals m.Id_Auftrag
                           join g in d.Glühung on m.Id_Glühung equals g.Id
                           where g.Id == request.GId
                           select new { o.ODL, o.Abmessung1, o.Id, m.Id_Auftrag, m.Gewicht, m.Anzahl }
                           );
                // .Take(request.Anzahl);

                //Gesamtanzahl Material in Glühung
                var gaz = (from j in d.Material
                           where j.Id_Glühung == request.GId
                           select j.Anzahl).Sum();

                if (ord.Count() > 0)
                {
                    byte[] ba = { };
                    foreach (var a in ord)
                    {
                        //Gewicht pro Auftrag in Glühung
                        var gew = (from i in ord
                                   where i.Id == a.Id_Auftrag
                                   select i.Gewicht
                                   ).Sum();
                        var anz = (from l in ord
                                   where l.Id == a.Id_Auftrag
                                   select l.Anzahl
                                   ).Sum();

                        ba.Concat(Encoding.ASCII.GetBytes(FixStringLenght(a.ODL + " " + a.Abmessung1 + "rd " + gew.Value + "Kg " + anz.Value + "/" + gaz.Value)));
                    }

                    SendAnswer(
                    n, BitConverter.GetBytes(Convert.ToInt16(request.Id))
                    .Concat(BitConverter.GetBytes(request.AId))
                    .Concat(ba)
                    .ToArray());
                }

            }
            if (request.Befehl.Contains(Request.STRING_GET_ITEMS_IN_ANNEALING))
            {
                var mat = from m in d.Material
                              // where m.Id_Auftrag == request.AId && m.Id_Glühung == request.GId
                          select m;


                if (mat.Count() > 0)
                {
                    byte[] ba = { };
                    foreach (var a in mat)
                    {
                        ba.Concat(Encoding.ASCII.GetBytes(FixStringLenght(a.Id + " " + a.Gewicht + "Kg")));
                    }

                    SendAnswer(
                    n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                    .Concat(ba)
                    .ToArray());
                    //int i = mat.Count();
                    //int s = 0;
                }
                if (request.Befehl.Contains(Request.STRING_GET_ITEMS_OUT_ANNEALING))
                {
                    var mot = from m in d.Material
                              where m.Id_Auftrag == request.AId && m.Status == "frei" // && m.Id_Glühung == request.GId
                              select m;


                    if (mot.Count() > 0)
                    {
                        byte[] ba = { };
                        foreach (var a in mot)
                        {
                            ba.Concat(BitConverter.GetBytes(a.Id));
                            ba.Concat(Encoding.ASCII.GetBytes(FixStringLenght(a.Id + " " + a.Gewicht + "Kg")));
                        }

                        SendAnswer(
                        n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                        .Concat(ba)
                        .ToArray());
                    }
                    if (request.Befehl.Contains(Request.STRING_MOVE_ALL_ITEMS_TO_RESERVE))
                    {
                        bool ok;
                        var mtl = from g in d.Material
                                  where g.Id == request.IId
                                  select g;

                        Material l = mtl.First();
                        l.Id_Glühung = 0;

                        try
                        {
                            d.SubmitChanges();
                            ok = true;
                        }
                        catch (Exception)
                        {
                            ok = false;
                            Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                        }

                        if (ok)
                        {
                            byte[] ba = Encoding.ASCII.GetBytes("CMD_OK");
                            SendAnswer(
                            n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                            .Concat(ba)
                            .ToArray());
                        }
                    }
                    if (request.Befehl.Contains(Request.STRING_MOVE_ITEM_TO_ANNEALING))
                    {
                        bool ok;
                        var mtl = from g in d.Material
                                  where g.Id == request.IId
                                  select g;

                        Material l = mtl.First();
                        l.Id_Glühung = request.GId;

                        try
                        {
                            d.SubmitChanges();
                            ok = true;
                        }
                        catch (Exception)
                        {
                            ok = false;
                            Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                        }

                        if (ok)
                        {
                            byte[] ba = Encoding.ASCII.GetBytes("CMD_OK");
                            SendAnswer(
                            n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                            .Concat(ba)
                            .ToArray());
                        }
                    }
                    if (request.Befehl.Contains(Request.STRING_MOVE_ALL_ITEMS_TO_RESERVE))
                    {
                        bool ok;
                        var mtl = from g in d.Material
                                  where g.Id_Auftrag == request.AId
                                  select g;

                        foreach (Material m in mtl)
                        {
                            m.Id_Glühung = 0;
                        }

                        try
                        {
                            d.SubmitChanges();
                            ok = true;
                        }
                        catch (Exception)
                        {
                            ok = false;
                            Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                        }

                        if (ok)
                        {
                            byte[] ba = Encoding.ASCII.GetBytes("CMD_OK");
                            SendAnswer(
                            n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                            .Concat(ba)
                            .ToArray());
                        }
                    }
                    if (request.Befehl.Contains(Request.STRING_MOVE_ALL_ITEMS_TO_ANNEALING))
                    {
                        bool ok;
                        var mtl = from g in d.Material
                                  where g.Id_Auftrag == request.AId
                                  select g;

                        foreach (Material m in mtl)
                        {
                            m.Id_Glühung = request.GId;
                        }

                        try
                        {
                            d.SubmitChanges();
                            ok = true;
                        }
                        catch (Exception)
                        {
                            ok = false;
                            Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                        }

                        if (ok)
                        {
                            byte[] ba = Encoding.ASCII.GetBytes("CMD_OK");
                            SendAnswer(
                            n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                            .Concat(ba)
                            .ToArray());
                        }
                    }
                    if (request.Befehl.Contains(Request.STRING_SET_ANNEALING_STAT))
                    {
                        bool ok;
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
                        try
                        {
                            d.SubmitChanges();
                            ok = true;
                        }
                        catch (Exception)
                        {
                            ok = false;
                            Console.WriteLine("Datenbankzugriff fehlgeschlagen!");
                        }

                        if (ok)
                        {
                            byte[] ba = Encoding.ASCII.GetBytes("CMD_OK");
                            SendAnswer(
                            n, BitConverter.GetBytes(Convert.ToInt16(request.Id))

                            .Concat(ba)
                            .ToArray());
                        }

                    }
                }
            }
        }

        private void Answer_Prozess()
        {
            if (request.Befehl.Contains(Request.STRING_GET_ASSIGNMENT))
            {

            }
            if (request.Befehl.Contains(Request.STRING_SET_ASSIGNMENT))
            {

            }
            if (request.Befehl.Contains(Request.STRING_GET_PROGRAM))
            {

            }
            if (request.Befehl.Contains(Request.STRING_SAVE_LOG_15S))
            {

            }
            if (request.Befehl.Contains(Request.STRING_SAVE_LOG_30S))
            {

            }
            if (request.Befehl.Contains(Request.STRING_SAVE_LOG_60S))
            {

            }
        }


    }
}
