using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsapp.Domain.Entities
{
        public class GroupRequestModel
        {
            public string groupId { get; set; }
            public IFormFile file { get; set; }
        }

    public class CreateGroupRequestModel
    {
        public string groupName { get; set; }
        public IFormFile file { get; set; }
    }
}