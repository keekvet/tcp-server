using MessageCore.Models;
using MessageRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class GetAllStoredMessagesController : BaseController
    {
        new public static void Handle(string data)
        {
            User sender = RequestConverter.DecomposeUser(data);
            List<Message> messages = MessageService.GetMessagesAdressedTo(sender.Name);

            try
            {
                Connection senderConnection = ConnectionController.Connections
                    .Where(c => c.ConnectedUser.Name == sender.Name).First();

                foreach (Message messageToSend in messages)
                {
                    
                    Package.Write(
                        senderConnection.TcpClient.GetStream(), 
                        RequestConverter.ComposeMessage(messageToSend));
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            
        }
    }
}
