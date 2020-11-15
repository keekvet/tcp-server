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
             User sender = context.Users
                .FirstOrDefault(u => u.Name == message.Sender.Name);
             User receiver= context.Users
                .FirstOrDefault(u => u.Name == message.Receiver.Name);
            
            if (sender != null)
                message.Sender = sender;
            if (receiver != null)
                message.Receiver = receiver;

            context.Messages.Add(message);
            context.SaveChanges();
        }    

        public static List<Message> GetMessagesAdressedTo(string userName)
        {

            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.Reload();
            }

            return context.Messages.Where(m => m.Receiver.Name == userName).ToList();

        }

        public static void DeleteMessagesWithReceiver(string userName)
        {
            context.Messages.RemoveRange(context.Messages.Where(m => m.Receiver.Name == userName));
            context.SaveChanges();
        }
    }
}
