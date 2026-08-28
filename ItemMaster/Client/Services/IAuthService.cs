using ItemMaster.Shared.Model;

namespace ItemMaster.Client.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> LoginAsync(LoginModel request);
        Task<ServiceResponse<int>> RegisterAsync(RegisterModel request);
        Task<ServiceResponse<string>> ForgotPasswordAsync(InputModel request);
        Task<ServiceResponse<string>> ResetPasswordAsync(InputModel request);

    }
}
