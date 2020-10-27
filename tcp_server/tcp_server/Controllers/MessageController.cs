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
        private MessageController() { }

        new public static void Handle(string data)
        {
            Message message = RequestConverter.DecomposeMessage(data);

            message.SendedTime = DateTime.UtcNow;

            try
            {
                Connection receiverConnection = ConnectionController.Connections
                    .Where(c => c.ConnectedUser.Name == message.Receiver.Name).First();

                Package.Write(
                        receiverConnection.TcpClient.GetStream(),
                        RequestConverter.ComposeMessage(message));
            }
            catch (InvalidOperationException)
            {
                MessageService.SaveMessage(message);
            }
        }

    };
}
