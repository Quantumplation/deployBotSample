using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace deployBotSample
{
    class Program
    {
        private static TcpListener listener;
        private static Thread listenThread;

        static void Main(string[] args)
        {
            var port = 31415;
            if (args.Count() == 1)
                port = Int32.Parse(args[0]);

            listener = new TcpListener(IPAddress.Any, port);
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
        }

        private static void ListenForClients()
        {
            listener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = listener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(HandleClientComm);
                clientThread.Start(client);
            }
        }

        private static void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("Hello Client!");

            Console.WriteLine("Test");

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            tcpClient.Close();
        }
    }
}
