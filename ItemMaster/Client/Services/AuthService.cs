using ItemMaster.Shared.Model;
using System.Net.Http.Json;

namespace ItemMaster.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<string>> ForgotPasswordAsync(InputModel request)
        {
            var result = await _http.PostAsJsonAsync("api/account/forgotPassword", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginModel request)
        {
            var result = await _http.PostAsJsonAsync("api/Account/login", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> RegisterAsync(RegisterModel request)
        {
            var result = await _http.PostAsJsonAsync("api/account/register", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<string>> ResetPasswordAsync(InputModel request)
        {
            var result = await _http.PostAsJsonAsync("api/account/resetPassword", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}
