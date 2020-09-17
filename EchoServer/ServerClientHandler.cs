using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EchoServer
{
    public class ServerClientHandler
    {
        private TcpClient clientSocket;
        private string clNo;
        private static Dictionary<int, TcpClient> clientsPool = new Dictionary<int, TcpClient>();
        public void startClient(TcpClient clientSocket, string clNo)
        {
           
            this.clientSocket = clientSocket;
            this.clNo = clNo;
            clientsPool.Add(Int32.Parse(clNo), clientSocket);
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            List<NetworkStream> networkStreams = new List<NetworkStream>();
            while (true)
            {
                // read
                byte[] dataFromClient = new byte[1024];
                int bytesRead = clientsPool[Int32.Parse(clNo)].GetStream().Read(dataFromClient, 0, dataFromClient.Length);
                string s = Encoding.ASCII.GetString(dataFromClient, 0, bytesRead);
                Message message = new Message();
                message.DeSerialize(s);
                Console.WriteLine($"Client {clNo}: {message.Message1}");
                if (s.Equals("exit"))
                {
                    break;
                }
                            
                //respond
                byte[] dataToClient = null;
                foreach (KeyValuePair<int, TcpClient> entry in clientsPool)
                {
                    if (entry.Key.Equals(Int32.Parse(clNo)))
                    {
                        dataToClient = Encoding.ASCII.GetBytes($"You say: {message.Message1}");
                    }
                    else
                    {
                        dataToClient = Encoding.ASCII.GetBytes($"Client {clNo} says: {message.Message1}");
                    }
                    entry.Value.GetStream().Write(dataToClient, 0 ,dataToClient.Length);
                }
            }

            clientsPool.Remove(Int32.Parse(clNo));
            //close connection
            clientSocket.Close();
        }
    }
}