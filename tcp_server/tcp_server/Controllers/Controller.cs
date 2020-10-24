using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tcp_server
{
    class Controller
    {
        private TcpClient client;
        private TcpListener server;

        public void RunListener()
        {
            int port = (int)Configuration.GetValue(typeof(int), "port");
            IPAddress ip = IPAddress.Parse((string)Configuration.GetValue(typeof(string), "IP"));

            server = new TcpListener(ip, port);
            server.Start();
        }

        public void getClient()
        {
            client = server.AcceptTcpClient();

            NetworkStream networkStream = client.GetStream();

            int i = 0;

            while (true)
            {
                Thread.Sleep(60000);

                networkStream.Write(Encoding.UTF8.GetBytes("welcome " + i++));
            }
            //networkStream.Close();
            //client.Close();
        }
    }

    class Connection
    {

    }
}
