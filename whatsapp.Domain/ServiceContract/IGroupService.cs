using Microsoft.AspNetCore.Http;
using whatsapp.Domain.DTOs;

namespace whatsapp.Domain.ServiceContract
{
    public interface IGroupService
    {
        public Task<List<GroupInfoResponseDto>> GetGroups();
        public Task<MemoryStream> AddUsersToGroup(string groupId, IFormFile file);
        public Task<MemoryStream> CreateGroup(string groupId, IFormFile file);
    }
}
