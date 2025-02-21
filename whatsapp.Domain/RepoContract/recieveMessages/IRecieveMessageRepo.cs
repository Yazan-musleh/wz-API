using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using whatsapp.Domain.Entities;

namespace whatsapp.Domain.RepoContract.recieveMessages
{
    public interface IRecieveMessageRepo
    {
        public  Message SaveMessage(Message message);
        public void SaveTextMessage(TextMessage message);
        public void SaveLocation(LocationMessage location);
        public Boolean IsMessageSent(string phoneNumber);
        public void Commit();
        public IDbContextTransaction BeginTransaction();
    }
}
