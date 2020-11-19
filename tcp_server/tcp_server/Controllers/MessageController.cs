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
                MessageService.SaveMessageIfLocalIdUnique(message);
                message = MessageService.GetMessageById(message.Id);
                
                Package.Write(
                    connection.TcpClient.GetStream(), 
                    RequestConverter.ComposeMessageSyncInfo(message.Id, message.SenderLocalId, message.SendedTime));

                Connection receiverConnection = ConnectionController.Connections
                    .FirstOrDefault(c => c.ConnectedUser.Name == message.Receiver);

                if (!(receiverConnection is null))
                    Package.Write(
                            receiverConnection.TcpClient.GetStream(),
                            RequestConverter.ComposeMessage(message));
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    //MessageService.SaveMessage(message);

                // here i got exception; i must correct it;
                //else
                //throw;

                Console.WriteLine(ex.Message);
            }
        }

    };
}
