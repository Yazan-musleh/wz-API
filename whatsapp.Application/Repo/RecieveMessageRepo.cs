using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using whatsapp.Domain.Entities;
using whatsapp.Domain.RepoContract.sentMessages;
using whatsapp.Infastructure.Database;

namespace whatsapp.Application.Repo
{
    public class SentMessageRepo : ISentMessageRepo
    {
        private readonly WhatsappApiContext _context;

        public SentMessageRepo(WhatsappApiContext context)
        {
            _context = context;
        }

        public Message SaveMessage(Message message)
        {
            _context.Messages.Add(message);
            return message;
        }


        public void SaveLocation(LocationMessage location)
        {
            _context.LocationMessages.Add(location);
        }


        public void SaveTextMessage(TextMessage message)
        {
           _context.TextMessages.Add(message);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public bool IsMessageSent(string phoneNumber)
        {
            bool receiverExists = _context.Messages.Any(m => m.Receiver == phoneNumber);

            return receiverExists;
        }
    }
}
