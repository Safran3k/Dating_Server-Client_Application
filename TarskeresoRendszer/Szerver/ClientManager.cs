using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Szerver
{
    static internal class ClientManager
    {
        static List<Meghivas> meghivasokLista = new List<Meghivas>();
        static List<Felhasznalo> felhasznalokLista = new List<Felhasznalo>();

        static ClientManager()
        {
            XDocument doc = XDocument.Load("datas.xml");

            foreach (XElement d in doc.Root.Descendants())
            {
                meghivasokLista.Add(new Meghivas(
                    d.Attribute("user1").Value,
                    d.Attribute("user2").Value,
                    d.Attribute("subject").Value,
                    d.Attribute("status").Value
                ));
            }
            XDocument udoc = XDocument.Load("felhasznalok.xml");

            foreach (XElement f in udoc.Root.Descendants())
            {
                felhasznalokLista.Add(new Felhasznalo(
                    f.Attribute("felhasznalonev").Value,
                    f.Attribute("jelszo").Value
                ));
            }
        }




        static internal void Start(TcpClient client)
        {
            Console.WriteLine("Egy kliens csatlakozott.");

            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            Felhasznalo aktFelhasznalo = null;

            while (!reader.EndOfStream)
            {
                string parancs = reader.ReadLine();
                string[] parancsParameterek = parancs.Split('|');

                switch (parancsParameterek[0])
                {
                    case "LOGIN":
                        try
                        {
                            aktFelhasznalo = felhasznalokLista.First(f => f.Felhasznalonev == parancsParameterek[1] && f.Jelszo == parancsParameterek[2]);
                            writer.WriteLine("Sikerült {0} felhasználónak belépni.", aktFelhasznalo.Felhasznalonev);
                        }
                        catch
                        {
                            writer.WriteLine("HIBÁS LOGIN!");
                        }
                        break;

                    case "LOGOUT":
                        if (aktFelhasznalo != null)
                        {
                            writer.WriteLine("{0} felhasználó kijelentkezett.", aktFelhasznalo.Felhasznalonev);
                            aktFelhasznalo = null;
                        }
                        else
                        {
                            writer.WriteLine("Kijelentkezéshez előbb be kell jelentkezni!");
                        }
                        break;

                    case "EXIT":
                        reader.Close();
                        writer.Close();
                        Console.WriteLine("Kilépett az egyik kliens");
                        return;

                    case "INVITE":
                        try
                        {
                            if (aktFelhasznalo != null)
                            {
                                Meghivas aktMeghivas = new Meghivas(aktFelhasznalo.Felhasznalonev, parancsParameterek[1], parancsParameterek[2], "pending");

                                Meghivas keresettMeghivas = meghivasokLista.Find(x => x.Felhasznalo1 == aktFelhasznalo.Felhasznalonev && x.Felhasznalo2 == parancsParameterek[1] || x.Felhasznalo2 == aktFelhasznalo.Felhasznalonev && x.Felhasznalo1 == parancsParameterek[1] && x.Statusz == "pending");

                                if (keresettMeghivas == null)
                                {
                                    meghivasokLista.Add(aktMeghivas);
                                }
                                else
                                {
                                    aktMeghivas = null;
                                    writer.WriteLine("Már van függőben lévő kapcsolat.");
                                }
                            }
                        }
                        catch (Exception)
                        {
                            writer.WriteLine("Ezen két felhasználó között már van kapcsolat.");
                        }

                        break;

                    case "ACCEPT":
                        if (aktFelhasznalo != null)
                        {
                            string meghivottSzemely = string.Empty;

                            Meghivas keresettMeghivas = meghivasokLista.Find(x => x.Felhasznalo1 == aktFelhasznalo.Felhasznalonev && x.Felhasznalo2 == parancsParameterek[1] || x.Felhasznalo2 == aktFelhasznalo.Felhasznalonev && x.Felhasznalo1 == parancsParameterek[1] && x.Statusz == "pending");

                            if (keresettMeghivas != null)
                            {
                                foreach (Meghivas item in meghivasokLista)
                                {
                                    if (keresettMeghivas == item)
                                    {
                                        item.Statusz = "accepted";

                                        if (aktFelhasznalo.Felhasznalonev == item.Felhasznalo1)
                                        {
                                            meghivottSzemely = item.Felhasznalo2;
                                        }
                                        else
                                        {
                                            meghivottSzemely = item.Felhasznalo1;
                                        }
                                    }

                                    writer.WriteLine($"Elfogadva {aktFelhasznalo.Felhasznalonev} - {meghivottSzemely} között a meghívás.");
                                    meghivottSzemely = string.Empty;
                                }

                                keresettMeghivas = null;
                            }
                            else
                            {
                                writer.WriteLine("Nem található ilyen kapcsolat.");
                            }
                        }
                        break;

                    case "REJECT":
                        if (aktFelhasznalo != null)
                        {
                            string meghivottSzemely = string.Empty;

                            Meghivas keresettMeghivas = meghivasokLista.Find(x => x.Felhasznalo1 == aktFelhasznalo.Felhasznalonev && x.Felhasznalo2 == parancsParameterek[1] || x.Felhasznalo2 == aktFelhasznalo.Felhasznalonev && x.Felhasznalo1 == parancsParameterek[1] && x.Statusz == "pending");

                            if (keresettMeghivas != null)
                            {
                                foreach (Meghivas item in meghivasokLista)
                                {
                                    if (keresettMeghivas == item)
                                    {
                                        item.Statusz = "rejected";

                                        if (aktFelhasznalo.Felhasznalonev == item.Felhasznalo1)
                                        {
                                            meghivottSzemely = item.Felhasznalo2;
                                        }
                                        else
                                        {
                                            meghivottSzemely = item.Felhasznalo1;
                                        }
                                    }

                                    writer.WriteLine($"Elutasítva {aktFelhasznalo.Felhasznalonev} - {meghivottSzemely} között a meghívás.");
                                }
                            }
                            else
                            {
                                writer.WriteLine("Nem található ilyen kapcsolat.");
                            }
                        }
                        break;

                    case "INVITATIONS":
                        if (aktFelhasznalo != null)
                        {
                            writer.WriteLine("###BEGIN###");
                            foreach (Meghivas item in meghivasokLista)
                            {
                                if (item.Felhasznalo1 == aktFelhasznalo.Felhasznalonev || item.Felhasznalo2 == aktFelhasznalo.Felhasznalonev)
                                {
                                    writer.WriteLine($"{item.Felhasznalo1} - {item.Felhasznalo2} Tárgy: {item.Targy} Státusz: {item.Statusz}");
                                }
                            }
                            writer.WriteLine("###END###");
                        }
                        break;

                    default:
                        writer.WriteLine("Ismeretlen parancs!");
                        break;
                }
            }
        }




    }
}
