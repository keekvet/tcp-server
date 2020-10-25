using MessageCore.Models;
using MessageRequest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tcp_server
{
    class MainController
    {
        private TcpListener server;

        public void Run()
        {
            int port = (int)Configuration.GetValue(typeof(int), "port");
            IPAddress ip = IPAddress.Parse((string)Configuration.GetValue(typeof(string), "IP"));

            server = new TcpListener(ip, port);
            server.Start();
            Console.WriteLine("started");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Connection connection = new Connection(client);

                Thread clientThread = new Thread(new ThreadStart(connection.StartSession));
                clientThread.Start();
            }
        }
    }

    class Connection
    {
        private TcpClient client;

        static private ApplicationContext context = ApplicationContext.getContext();

        private User user;
        public User ConnectedUser
        {
            get { return user; }
        }

        public Connection(TcpClient client)
        {
            this.client = client;
        }

        public void StartSession()
        {
            int bytesRead;
            byte[] buffer = new byte[1024];

            NetworkStream networkStream = null;

            try
            {
                Console.WriteLine("new client");

                networkStream = client.GetStream();

                bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                string aliveData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (user is null && (RequestConverter.GetRequestType(aliveData) == RequestType.Alive))
                {
                    user = RequestConverter.DecomposeUser(RequestConverter.GetData(aliveData));
                }



                if (context.Users.Contains(user))
                {
                    Console.WriteLine("found");
                    networkStream.Write(Encoding.UTF8.GetBytes("welcome " + user.Name));
                }
                else
                {
                    Console.WriteLine("not found");
                }


                networkStream.Write(Encoding.UTF8.GetBytes("how are you?"));
            }
            catch (IOException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            finally
            {
                if (networkStream is null)
                    networkStream.Close();
                if (client is null)
                    client.Close();

                Console.WriteLine(user.Name + " session ended");
            }
        }
    }
}
