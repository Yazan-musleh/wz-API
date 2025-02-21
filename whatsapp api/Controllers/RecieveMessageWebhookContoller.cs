using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using whatsapp.Domain.ServiceContract;

namespace whatsapp_api.Controllers
{

    [ApiController]
    [Route("api/webhook")]
    public class RecieveMessageWebhookContoller : ControllerBase
    {

        private readonly IRecieveMessageService _service;

        public RecieveMessageWebhookContoller(IRecieveMessageService service)
        {
            _service = service;
        }

        private static readonly Dictionary<string, string> SecurityTokens = new()
        {
            { "41849", "5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574" } // Example instanceId and token
        };

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook(string securityToken, [FromBody] JsonElement payload)
        {
            try
            {
                Console.WriteLine($"Received webhook: {payload}");

                // Extract required fields safely
                if (!payload.TryGetProperty("instanceId", out var instanceIdElement) ||
                    !payload.TryGetProperty("event", out var eventElement) ||
                    !payload.TryGetProperty("data", out var dataElement))
                {
                    Console.WriteLine("Invalid request structure.");
                    return BadRequest("Missing required fields.");
                }

                string instanceId = instanceIdElement.GetString();
                string eventName = eventElement.GetString();

                // Validate security token
                if (!SecurityTokens.ContainsKey(instanceId) || SecurityTokens[instanceId] != securityToken)
                {
                    Console.WriteLine("Authentication failed.");
                    return Unauthorized();
                }

                // Process message event
                if (eventName == "message")
                {
                    Console.WriteLine("Processing message event...");

                    if (dataElement.TryGetProperty("message", out var messageData))
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

                        Console.WriteLine($"📩 New Message of type {messageType} and id {messageId}" +
                            $" from {senderPhone} to {recieverId}: {messageContent} at {messageTime}");
                        
                        _service.saveRecievedMessage(messageData);
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown event: {eventName}");
                    return NotFound("Event not handled.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing webhook: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
