using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsapp.Domain.ServiceContract
{
    public interface IMessageService
    {
        public Task<string> SendBulkMessages(List<string> contacts, string message);
        public Task<string> SendBulkMediaMessages(List<string> contacts,string mediaUrl, string message);
    }
}
