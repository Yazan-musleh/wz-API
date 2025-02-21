using Azure;
using RestSharp;
using System.Text.Json;
using whatsapp.Domain.Entities;
using whatsapp.Domain.RepoContract.recieveMessages;
using whatsapp.Domain.ServiceContract;
using whatsapp.Infastructure.Database;

namespace whatsapp.Application.Services
{
    public class MessageService : IMessageService
    {

        private readonly IRecieveMessageRepo _repo;

        public MessageService(IRecieveMessageRepo repo)
        {
            _repo = repo;
        }

        public async Task<string> SendBulkMediaMessages(List<string> contacts, string mediaUrl, string message)
        {

            RestResponse response = null;


            var tasks = contacts.Select(async contacts =>
            {

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/send-media");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
                var payload = new
                {
                    chatId = contacts,  // Must be in international format without "+"
                    mediaUrl = mediaUrl,
                    mediaCaption = message
                };
                request.AddJsonBody(payload);

                response = await client.PostAsync(request);

            Console.WriteLine("{0}", response.Content);

                await SaveToDb(response, contacts);

            });

            await Task.WhenAll(tasks);

            return "";
        }

        public async Task<string> SendBulkMessages(List<string> contacts, string message)
        {

            var options = new RestClientOptions("https://waapi.app/api/v1/instances/41849/client/action/send-message");

            var tasks = contacts.Select(async contacts =>
            {
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", "Bearer 5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574");
                request.AddJsonBody(new { chatId = contacts, message = message });
                var response = await client.PostAsync(request);
            Console.WriteLine("{0}", response.Content);

                await SaveToDb(response, contacts);

            });

            await Task.WhenAll(tasks);

            return "";

        }

        public async Task SaveToDb(RestResponse response, string contacts)
        {
            if (response.IsSuccessStatusCode)
            {
                
                using (JsonDocument doc = JsonDocument.Parse(response.Content))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement messageData = root.GetProperty("data").GetProperty("data").GetProperty("_data");

                    try
                    {
                        using (var context = new WhatsappApiContext())
                        {
                            // Assuming you are parsing the response and extracting necessary info
                            Message messageEntity = new Message
                            {
                                WaMessageId = messageData.GetProperty("id").GetProperty("id").GetString(),
                                Receiver = messageData.GetProperty("to").GetProperty("user").GetString(),
                                Sender = messageData.GetProperty("from").GetProperty("user").GetString(),
                                MessageType = messageData.GetProperty("type").GetString(),
                                LocalTime = DateTime.Now

                            };

                            // Save the message to the database asynchronously
                            context.Messages.Add(messageEntity);
                            await context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving message: {ex.Message}");
                    }
                }
            }
        }
    }
}
