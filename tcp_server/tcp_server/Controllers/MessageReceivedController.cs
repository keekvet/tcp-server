using MessageCore.Models;
using MessageRequest;
using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class MessageReceivedController : BaseController
    {
        new public static void Handle(string data, Connection connection)
        {
            Message message = MessageService.GetMessageById(RequestConverter.DecomposeMessageReceived(data));
            message.State |= MessageState.ReceiverReceived;

            MessageService.CheckAndHandleMessageState(message);
        }
    }
}
