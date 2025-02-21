using System.Text.Json;

namespace whatsapp.Domain.ServiceContract
{
    public interface IRecieveMessageService
    {
        public void saveRecievedMessage(JsonElement recievedMessagePayload);
    }
}
