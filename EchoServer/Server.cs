using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server..");
            
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpClient client = default(TcpClient);
            TcpListener serverSocket = new TcpListener(ip, 5000);
            int counter = 0;
            serverSocket.Start();
            Console.WriteLine("Server started..");

            try
            {
                while (true)
                {
                    counter += 1;
                    client = serverSocket.AcceptTcpClient();
                    Console.WriteLine($"Client number {counter} connects");
                    Console.WriteLine("Client connected");
                    ServerClientHandler clientHandler = new ServerClientHandler();
                    clientHandler.startClient(client, Convert.ToString(counter));
                }
            }
            catch (Exception e)
            {
                client.Close();
                serverSocket.Stop();
                Console.WriteLine("Server is closed");
            }
            client.Close();
        }
    }
}

