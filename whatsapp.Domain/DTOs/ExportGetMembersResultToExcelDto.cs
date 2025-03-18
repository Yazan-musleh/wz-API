using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsapp.Domain.DTOs
{
    public class ExportGetMembersResultToExcelDto
    {
            public string PhoneNumber { get; set; }
            public bool IsAdmin { get; set; }
    }
}
