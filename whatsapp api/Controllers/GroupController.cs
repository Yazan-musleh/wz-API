using Microsoft.AspNetCore.Mvc;
using System.IO;
using whatsapp.Domain.Entities;
using whatsapp.Domain.ServiceContract;

namespace whatsapp_api.Controllers
{


    [ApiController]
    [Route("api/group")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("getGroups")]
        public async Task<ActionResult> GetGroups()
        {
            return Ok(await _groupService.GetGroups());
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUsersToGroup(IFormFile file, [FromQuery] string groupId)
        {
            //var result = await _groupService.AddUsersToGroup(request.groupId, request.file);
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

           
                var result = await _groupService.AddUsersToGroup(groupId, file);
                Console.WriteLine(result);
                return File(result.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WhatsAppUserData.xlsx");

            
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupMembers(string groupId)
        {
            try
            {

                var result = await _groupService.GetGroupMembers(groupId);

                return File(result.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WhatsAppUserData.xlsx");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error processing file: {ex.Message}");
            }
        }

            
        [HttpPost("Create")]
        public async Task<IActionResult> CreateGroup(IFormFile file, [FromQuery] string groupName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var result = await _groupService.CreateGroup(groupName, file);
                Console.WriteLine(result);
                return File(result.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WhatsAppUserData.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing file: {ex.Message}");
            }
            
        }

    }

}
