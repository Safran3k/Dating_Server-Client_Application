using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Szerver
{
    class Program
    {
        static void Kilepes(TcpListener listener)
        {
            while (true)
            {
                TcpClient client;
                lock (listener)
                {
                    client = listener.AcceptTcpClient();
                }
                Thread t = new Thread(() => ClientManager.Start(client));
                t.Start();
            }
        }


        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(ConfigurationManager.AppSettings["IP"]);
            int port = int.Parse(ConfigurationManager.AppSettings["PORT"]);

            TcpListener listener = new TcpListener(ip, port);

            listener.Start();

            Thread t = new Thread(() => Kilepes(listener));
            t.Start();

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Megnyomtam.");
            }

            Environment.Exit(0);
            t.Join();
            listener.Stop();


        }
    }
}
