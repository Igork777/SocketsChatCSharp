using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EchoServer
{
    public class Client
    {
        private static NetworkStream stream;
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting client..");
            
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            Message message;
            stream = client.GetStream();
            Console.Write("Input the message->\n");
            do
            {
                Thread thread = new Thread(waitingForTheMessage);
                thread.Start();
               message = new Message(Console.ReadLine());
                byte[] dataToServer = Encoding.ASCII.GetBytes(message.Serialize());
                stream.Write(dataToServer, 0, dataToServer.Length);
            } while (!(message.Equals("exit")));
            
            stream.Close();
            client.Close();
        }

        public static void waitingForTheMessage()
        {
            while (true)
            {
                byte[] dataFromServer = new byte[1024];
                int bytesRead = stream.Read(dataFromServer, 0, dataFromServer.Length);
                string response = Encoding.ASCII.GetString(dataFromServer, 0, bytesRead);
                // Message message = new Message();
                // message.DeSerialize(response);
                Console.WriteLine(response);
                Console.Write("Input the message->\n");
            }
        }
    }
}