using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Kliens
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = ConfigurationManager.AppSettings["IP"];
            int port = int.Parse(ConfigurationManager.AppSettings["PORT"]);

            TcpClient client = new TcpClient(ip, port);

            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            Console.WriteLine("Írjon be egy parancsot!");
            while (true) 
            {
                Console.Write("$:/user> ");
                string parancs = Console.ReadLine();
                writer.WriteLine(parancs);

                if (parancs == "EXIT")
                {
                    break;
                }

                string valasz = reader.ReadLine();
                if (valasz == "###BEGIN###")
                {
                    valasz = reader.ReadLine();
                    while (valasz != "###END###")
                    {
                        Console.WriteLine(valasz);
                        valasz = reader.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine(valasz);
                }
            }

            reader.Close();
            writer.Close();
        }
    }
}
