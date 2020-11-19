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
                .FirstOrDefault(u => u.Name == message.Sender);
             User receiver= context.Users
                .FirstOrDefault(u => u.Name == message.Receiver);

            if (sender is null || receiver is null)
                return;

            context.Messages.Add(message);
            context.SaveChanges();
        }

        public static void CheckAndHandleMessageState(Message message)
        {
            if (message.State == (MessageState.SenderReceived | MessageState.ReceiverReceived))
                DeleteMessageById(message.Id);
            else
            {
                context.Messages.Update(message);
                context.SaveChanges();
            }
        }

        public static Message GetMessageById(int id)
        {
            return context.Messages.Find(id);
        }

        // if not exist message with same id and local id
        public static void SaveMessageIfLocalIdUnique(Message message)
        {
            if (GetMessageBySenderLocalId(message.SenderLocalId)?.Id != message.Id)
                SaveMessage(message);
        }

        private static Message GetMessageBySenderLocalId(int localId)
        {
            return context.Messages.Where(m => m.SenderLocalId == localId).FirstOrDefault();
        }

        public static List<Message> GetMessagesAdressedTo(string userName)
        {

            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.Reload();
            }

            return context.Messages.Where(m => m.Receiver == userName).ToList();
        }

        public static void DeleteMessagesWithReceiver(string userName)
        {
            context.Messages.RemoveRange(context.Messages.Where(m => m.Receiver == userName));
            context.SaveChanges();
        }

        public static void DeleteMessageById(int id)
        {
            context.Messages.RemoveRange(context.Messages.Where(m => m.Id == id));
            context.SaveChanges();
        }
    }
}
