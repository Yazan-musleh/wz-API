using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using whatsapp.Domain.Entities;
using whatsapp.Domain.RepoContract.recieveMessages;
using whatsapp.Domain.ServiceContract;

namespace whatsapp.Application.Services
{
    public class RecievedMessageService : IRecieveMessageService
    {

        private readonly IRecieveMessageRepo _repo;

        public RecievedMessageService(IRecieveMessageRepo repo)
        {
            _repo = repo;
        }

        public void saveRecievedMessage(JsonElement messageData)
        {
            string messageId = messageData.GetProperty("id")
                .GetProperty("id").ToString();
            string senderId = messageData.GetProperty("from").GetString();
            string recieverId = messageData.GetProperty("to").GetString();
            string messageType = messageData.GetProperty("type").GetString();
            string messageContent = messageData.GetProperty("body").GetString();
            long timestamp = messageData.GetProperty("timestamp").GetInt64();
            DateTime messageTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;

            string messageDataType = messageType == "chat" ? "Text Message" :
             messageType == "location" ? "Location" : "Unknown";

            string senderPhone = senderId.Replace("@c.us", "");
            string recieverPhone = recieverId.Replace("@c.us", "");

            if (messageDataType == "Unknown")
            {
                return;
            }

            if (!_repo.IsMessageSent(senderPhone))
            {
                return ;
            }

            using (var transaction = _repo.BeginTransaction()) // Start transaction
            {
                try
                {
                    Message message = new Message
                    {
                        Sender = senderPhone,
                        Receiver = recieverPhone,
                        MessageType = messageDataType,
                        LocalTime = DateTime.Now,
                        WaMessageId = messageId,
                    };


                    _repo.SaveMessage(message);
                    _repo.Commit(); // Ensure message ID is available



                    if (messageType == "chat")
                    {
                        TextMessage textMessage = new TextMessage
                        {
                            Id = message.Id,
                            MessageBody = messageContent
                        };

                        _repo.SaveTextMessage(textMessage);
                    }

                    if (messageType == "location")
                    {
                        double latitude = messageData.GetProperty("_data").GetProperty("lat").GetDouble();
                        double longitude = messageData.GetProperty("_data").GetProperty("lng").GetDouble();

                        LocationMessage location = new LocationMessage
                        {
                            Id = message.Id,
                            Latitude = latitude,
                            Longitude = longitude
                        };

                        _repo.SaveLocation(location);
                        _repo.Commit();
                    }

                    _repo.Commit();

                    transaction.Commit(); // Save everything only if all operations succeed
                }
                catch
                {
                    transaction.Rollback(); // Undo all changes if an error occurs
                    throw;
                }
            }


           


        }
    }
}
