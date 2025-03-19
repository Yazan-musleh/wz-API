using Microsoft.EntityFrameworkCore.Storage;
using whatsapp.Domain.Entities;

namespace whatsapp.Domain.RepoContract.sentMessages
{
    public interface ISentMessageRepo
    {
        public  Message SaveMessage(Message message);
        public void SaveTextMessage(TextMessage message);
        public void SaveLocation(LocationMessage location);
        public Boolean IsMessageSent(string phoneNumber);
        public void Commit();
        public IDbContextTransaction BeginTransaction();
    }
}
