using MessageCore.Models;
using MessageRequest;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class MessageController : BaseController
    {
        new public static void Handle(string data, Connection connection)
        {
            Message message = RequestConverter.DecomposeMessage(data);

            message.SendedTime = DateTime.UtcNow;

            try
            {
                Connection receiverConnection = ConnectionController.Connections
                    .FirstOrDefault(c => c.ConnectedUser.Name == message.Receiver.Name);

                if (receiverConnection is null)
                    MessageService.SaveMessage(message);
                else
                Package.Write(
                        receiverConnection.TcpClient.GetStream(),
                        RequestConverter.ComposeMessage(message));
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    MessageService.SaveMessage(message);
                
                // here i got exception; i must correct it;
                //else
                    //throw;

                Console.WriteLine(ex.Message);
            }
        }

    };
}
