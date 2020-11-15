using MessageCore.Models;
using MessageRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class GetAllStoredMessagesController : BaseController
    {
        new public static void Handle(string data, Connection connection)
        {
            List<Message> messages = MessageService.GetMessagesAdressedTo(connection.ConnectedUser.Name);

            try
            {

                foreach (Message messageToSend in messages)
                {

                    Package.Write(
                        connection.TcpClient.GetStream(),
                        RequestConverter.ComposeMessage(messageToSend));
                }
                Thread.Sleep(10);
                Package.Write(
                    connection.TcpClient.GetStream(),
                    RequestConverter.ComposeGetAllMessagesResponse());
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    Console.WriteLine(ex);
                else
                    throw;
            }
        }
    }
}
