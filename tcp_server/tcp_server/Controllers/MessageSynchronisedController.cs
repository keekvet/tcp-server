using MessageCore.Models;
using MessageRequest;
using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class MessageSynchronisedController : BaseController
    {
        new public static void Handle(string data, Connection connection)
        {
            Message message = MessageService.GetMessageById(RequestConverter.DecomposeMessageSynchronised(data));
            message.State |= MessageState.SenderReceived;

            MessageService.CheckAndHandleMessageState(message);
        }
    }
}
