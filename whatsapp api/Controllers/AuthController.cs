using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using whatsapp.Domain.ServiceContract;
using whatsapp.Infastructure.Services;

namespace whatsapp_api.Controllers;

[ApiController]
[Route("api/auth/webhook")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IHubContext<WaHub> _hubContext;
    
    
    private static readonly Dictionary<string, string> SecurityTokens = new()
        {
            { "41849", "5bXTf9sHQCO2TBfIlZWfI6T8T8UlHFNsOKa7CjuIf14b7574" } // Example instanceId and token
        };

    public AuthController(IAuthService authService, IHubContext<WaHub> hubContext)
    {
        _authService = authService;
        _hubContext = hubContext;
    }



    [HttpPost]
    public async Task<IActionResult> ReceiveWebhook(string securityToken, [FromBody] JsonElement payload)
    {
        try
        {
            Console.WriteLine($"Received webhook: {payload}");

            // Extract required fields safely
            if (!payload.TryGetProperty("instanceId", out var instanceIdElement) ||
                !payload.TryGetProperty("event", out var eventElement) )
            {
                Console.WriteLine("Invalid request structure.");
                return BadRequest("Missing required fields.");
            }

            

            string instanceId = instanceIdElement.GetString();
            string eventName = eventElement.GetString();

            if (eventName == "qr" || eventName == "loading_screen")
            {
                
            }
            

            // Validate security token
            if (!SecurityTokens.ContainsKey(instanceId) || SecurityTokens[instanceId] != securityToken)
            {
                Console.WriteLine("Authentication failed.");
                return Unauthorized();
            }

            /////////////// handle the event based on it's type \\\\\\\\\\\\\\\\

            // Process message event
            if (eventName == "qr")
            {
                if (!payload.TryGetProperty("data", out var dataElement))
                {
                    Console.WriteLine("Invalid request structure.");
                    return BadRequest("Missing required fields.");
                }

                Console.WriteLine("Processing qr event...");

                if (dataElement.TryGetProperty("qr", out var qrData))
                {
                    

                    var qrCode = dataElement.GetProperty("base64");

                    await _hubContext.Clients.All.SendAsync("ReceiveWebhookData", payload);

                    Console.WriteLine($"📩 New qr code with base64 {qrCode}");
                }
            }
            else if (eventName == "loading_screen")
            {

                if (!payload.TryGetProperty("data", out var dataElement))
                {
                    Console.WriteLine("Invalid request structure.");

                    return BadRequest("Missing required fields.");
                }

                Console.WriteLine("Processing loading screen event...");

                if (dataElement.TryGetProperty("percent", out var loading_percentage) 
                    && dataElement.TryGetProperty("message", out var loading_message))
                {
                    Console.WriteLine("loading percentage: ", loading_percentage);
                    Console.WriteLine("loading Message: ", loading_message);
                }

                await _hubContext.Clients.All.SendAsync("ReceiveWebhookData", payload);

            }
            else if (eventName == "ready")
            {
                Console.WriteLine("Processing instance ready...");
                await _hubContext.Clients.All.SendAsync("ReceiveWebhookData", payload);

            }
            else if (eventName == "authenticated")
            {
                Console.WriteLine("Processing authenticated event...");
                await _hubContext.Clients.All.SendAsync("ReceiveWebhookData", payload);
            }
            else if (eventName == "auth_failure")
            {
                    await _hubContext.Clients.All.SendAsync("ReceiveWebhookData", payload);
                Console.WriteLine("Processing authentication failure event...");
            }
            else if (eventName == "disconnected")
            {
                    await _hubContext.Clients.All.SendAsync("ReceiveWebhookData", payload);
                Console.WriteLine("Processing disconnection failure event...");
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

    [HttpGet("qr")]
    public async Task<IActionResult> GetQRCode()
    {
        var result = await _authService.GetQRCode();
        return Ok(result);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var result = await _authService.GetStatus();
        return Ok(result);
    }

    [HttpGet("user_data")]
    public async Task<IActionResult> RetrieveUserData()
    {
        var result = await _authService.RetrieveUserData();
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.Logout();
        return Ok(result);
    }
}

