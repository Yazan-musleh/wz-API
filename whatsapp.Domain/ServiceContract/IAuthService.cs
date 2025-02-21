using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsapp.Domain.ServiceContract
{
    public interface IAuthService
    {
        public Task<string> Logout();
        public Task<string> GetQRCode();
        public Task<string> GetStatus();
        public Task<string> RetrieveUserData();
    }
}
