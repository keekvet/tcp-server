using MessageCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Services
{
    class MessageService : BaseService
    {
        public static void SaveMessage(Message message)
        {
            message.Sender = context.Users
                .FirstOrDefault(u => u.Name == message.Sender.Name);
            message.Receiver = context.Users
                .FirstOrDefault(u => u.Name == message.Receiver.Name);
            context.Messages.Add(message);
            context.SaveChanges();
        }    

        public static List<Message> GetMessagesAdressedTo(string userName)
        {
            return context.Messages.Where(m => m.Receiver.Name == userName).ToList();
        }
    }
}
