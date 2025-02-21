using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whatsapp.Domain.DTOs
{
    public class AddMembersJsonResponse
    {
        public DataWrapper Data { get; set; }
        public string Status { get; set; }
    }

    public class DataWrapper
    {
        public string Status { get; set; }
        public string InstanceId { get; set; }
        public DataResult Data { get; set; }
    }

    public class DataResult
    {
        public Dictionary<string, UserResult> Result { get; set; }
    }

    public class UserResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsInviteV4Sent { get; set; }
    }

    public class ExportAddMembersResultToExcelDto
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }

}
