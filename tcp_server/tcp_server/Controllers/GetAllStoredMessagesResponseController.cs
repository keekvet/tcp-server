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
    class GetAllStoredMessagesResponseController : BaseController
    {
        new public static void Handle(string data, Connection connection)
        {
            MessageService.DeleteMessagesWithReceiver(connection.ConnectedUser.Name);
        }
    }
}
