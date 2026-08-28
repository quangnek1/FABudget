using ItemMaster.Server.Data.Entities;
using ItemMaster.Server.Extensions;
using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
    public interface IUtilityService
    {
        Task<User> GetUserAsync();
        Task<UserVm> GetByIdAsync(string id);
        Task<string> GetUserId();
        Task<string> GetDivisionName(string id);
        Task<IEnumerable<UserVm>> GetListUserAsync();
        Task<IEnumerable<EmailSendVm>> GetListUserSendMailAsync();
    }
}
