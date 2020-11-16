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

        public static ConcurrentBag<Connection> Connections { get; private set; }

        public ConnectionController()
        {
            Connections = new ConcurrentBag<Connection>();
        }

        public void Run()
        {
            ApplicationContext context = ApplicationContext.getContext();

            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.Reload();
            }

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
        public TcpClient TcpClient { get; private set; }
        public User ConnectedUser { get; set; }

        public Connection(TcpClient client) { TcpClient = client; }

        public void StartSession()
        {
            string receivedPackage;

            NetworkStream networkStream = null;

            try
            {
                Console.WriteLine("new client");

                networkStream = TcpClient.GetStream();

                while (true)
                {
                    receivedPackage = Package.Read(networkStream);
                    RequestType type;
                    try
                    {
                        type = RequestConverter.GetRequestType(receivedPackage);
                        Console.WriteLine(type);
                        Type selectedController = GetControllerByRequestType(type);

                        selectedController.GetMethod(nameof(BaseController.Handle))
                            .Invoke(null, new object[] { RequestConverter.GetData(receivedPackage), this });
                    }
                    catch (ControllerNotFoundExeption contrNotFound)
                    {
                        Console.WriteLine(contrNotFound.Message);
                    }
                }

            }
            catch (IOException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            catch (FormatException foEx)
            {
                Console.WriteLine(foEx.Message);
            }
            finally
            {
                networkStream?.Close();
                TcpClient?.Close();

                Connection connection = this;
                ConnectionController.Connections.TryTake(out connection);

                Console.WriteLine(ConnectedUser.Name + " session ended");
            }
        }

        private Type GetControllerByRequestType(RequestType type)
        {
            if (ConnectedUser is null)
                switch (type)
                {
                    case RequestType.Login:
                        return typeof(LoginController);
                    case RequestType.Registration:
                        return typeof(LoginController);
                    default:
                        throw new ControllerNotFoundExeption("not authenticated user");
                }
            else
                switch (type)
                {
                    case RequestType.SendMessage:
                        return typeof(MessageController);
                    case RequestType.GetAllStoredMessages:
                        return typeof(GetAllStoredMessagesController);
                    case RequestType.GetAllStoredMessagesResponse:
                        return typeof(GetAllStoredMessagesResponseController);
                    case RequestType.CheckUserExist:
                        return typeof(CheckUserExistController);
                    default:
                        throw new ControllerNotFoundExeption("controller for request not found");
                }
        }
    }

    class ControllerNotFoundExeption : Exception
    {
        public ControllerNotFoundExeption() { }
        public ControllerNotFoundExeption(string message) : base(message) { }
        public ControllerNotFoundExeption(string message, Exception inner) : base(message, inner) { }
    }
}
