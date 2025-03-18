using Microsoft.AspNetCore.Http;

namespace whatsapp.Domain.Entities
{
    public class MessageRequestModel
    {
        public IFormFile File { get; set; }
        public string Message { get; set; }
    }

    public class MediaMessageRequestModel: MessageRequestModel
    {
        public string mediaUrl{ get; set; }
    }
}
