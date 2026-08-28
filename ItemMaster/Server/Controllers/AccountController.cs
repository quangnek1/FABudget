using ItemMaster.Server.Services;
using ItemMaster.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace ItemMaster.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUtilityService _utilityService;
        public AccountController(IAuthRepository authRepo, IUtilityService utilityService)
        {
            _authRepo = authRepo;
            _utilityService = utilityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel request)
        {
            var response = await _authRepo.LoginAsync(request.Code, request.Password);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpGet("WeatherForecast")]
        public async Task<IActionResult> UserDetails()
        {
            return Ok();
        }
        [HttpGet("ListUser")]
        public async Task<IActionResult> UserListUser()
        {
            var response = await _utilityService.GetListUserAsync();
            return Ok(response);
        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(InputModel request)
        {
            var response = await _authRepo.ForgotPasswordAsync(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(InputModel request)
        {
            var response = await _authRepo.ResetPasswordAsync(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
