using MessageCore.Models;
using MessageRequest;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using tcp_server.Controllers;

namespace tcp_server
{
    class ConnectionController
    {
        private TcpListener server;

        private static ConcurrentBag<Connection> connections;
        public static ConcurrentBag<Connection> Connections { get => connections; }

        public ConnectionController()
        {
            connections = new ConcurrentBag<Connection>();
        }

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

                Connections.Add(connection);

                Thread clientThread = new Thread(new ThreadStart(connection.StartSession));
                clientThread.Start();
            }
        }
    }

    class Connection
    {
        private TcpClient tcpClient;
        public TcpClient TcpClient
        {
            get { return tcpClient; }
        }


        private User user;
        public User ConnectedUser
        {
            get { return user; }
        }

        public Connection(TcpClient client)
        {
            this.tcpClient = client;
        }

        public void StartSession()
        {
            string receivedPackage;

            NetworkStream networkStream = null;

            try
            {
                Console.WriteLine("new client");

                networkStream = tcpClient.GetStream();

                receivedPackage = Package.Read(networkStream);

                if (user is null && (RequestConverter.GetRequestType(receivedPackage) == RequestType.GetAllStoredMessages))
                {
                    user = RequestConverter.DecomposeUser(RequestConverter.GetData(receivedPackage));
                    GetAllStoredMessagesController.Handle(RequestConverter.GetData(receivedPackage));
                }

                while (true)
                {
                    receivedPackage = Package.Read(networkStream);

                    Console.WriteLine("received package: " + receivedPackage);
                    RequestType type = RequestConverter.GetRequestType(receivedPackage);

                    Type selectedController = GetControllerByRequestType(type);
                    Console.WriteLine(nameof(BaseController.Handle));
                    if(selectedController != null)
                        selectedController.GetMethod(nameof(BaseController.Handle))
                            .Invoke(null, new object[] { RequestConverter.GetData(receivedPackage) } );
                
                }

            }
            catch (IOException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            finally
            {
                if (networkStream != null)
                    networkStream.Close();
                if (tcpClient != null)
                    tcpClient.Close();

                Connection connection = this;
                ConnectionController.Connections.TryTake(out connection);

                Console.WriteLine(user.Name + " session ended");
            }
        }

        //private string Read()
        //{
        //    int bytesRead;
        //    bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
        //    return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //}

        private Type GetControllerByRequestType(RequestType type)
        {
            switch (type)
            {
                //case RequestType.Alive:
                //    break;
                case RequestType.Registration:
                    return typeof(RegistrationController);
                case RequestType.SendMessage:
                    return typeof(MessageController);
                case RequestType.GetAllStoredMessages:
                    return typeof(GetAllStoredMessagesController);
                //case RequestType.CheckUserExist:
                //    break;
                //case RequestType.CheckUserExistResult:
                //    break;
                default:
                    return null;
            }
        }
    }
}
