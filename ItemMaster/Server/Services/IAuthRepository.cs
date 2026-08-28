using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> RegisterAsync(User user, string password, int startUnitId);
        Task<ServiceResponse<string>> LoginAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
        Task<ServiceResponse<string>> ForgotPasswordAsync(InputModel model);
        Task<ServiceResponse<string>> ResetPasswordAsync(InputModel model);

    }
}
