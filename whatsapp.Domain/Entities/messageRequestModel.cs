namespace whatsapp.Domain.Entities
{
    public class MessageRequestModel
    {
        public List<string> Contacts { get; set; }
        public string Message { get; set; }
    }

    public class MediaMessageRequestModel: MessageRequestModel
    {
        public string mediaUrl{ get; set; }
    }
}
