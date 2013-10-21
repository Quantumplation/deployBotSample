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
            listener = new TcpListener(IPAddress.Any, 31415);
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
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private static void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("Hello Client! asasd");

            Console.WriteLine("Test asdfd");

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            tcpClient.Close();
        }
    }
}
