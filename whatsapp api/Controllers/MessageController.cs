using Microsoft.AspNetCore.Mvc;
using whatsapp.Application.Services;
using whatsapp.Domain.Entities;
using whatsapp.Domain.ServiceContract;

namespace whatsapp_api.Controllers
{

    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendBulkMessages([FromBody] MessageRequestModel request)
        {
            var result = await _messageService.SendBulkMessages(request.Contacts, request.Message);
            return Ok(result);
        }

        [HttpPost("send_image")]
        public async Task<IActionResult> SendImage([FromBody] MediaMessageRequestModel request)
        {
            var result = await 
                _messageService.SendBulkMediaMessages(request.Contacts, request.mediaUrl, request.Message);

            return Ok(result);
        }
    }

}
