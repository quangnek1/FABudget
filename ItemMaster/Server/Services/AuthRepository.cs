using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Server.Extensions;
using ItemMaster.Shared.Extensions;
using ItemMaster.Shared.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ItemMaster.Server.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ISendmailServices _sendmailServices;
        public AuthRepository(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, ISendmailServices sendmailServices)
        {
            _context = context;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _sendmailServices = sendmailServices;
        }

        public async Task<ServiceResponse<string>> LoginAsync(string code, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(code, password, false, false);

            var user3 = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName.ToLower() == code.ToLower());

            if (result == null)
            {
                return FailedLoginResponse();
            }

            //if (!VerifyPasswordHash(password, result.PasswordHash, result.PasswordHash)
            //{
            //    return FailedLoginResponse();
            //}
            var user = await _userManager.FindByNameAsync(code);
            var response = new ServiceResponse<string>()
            {
                IsSuccess = true,
                Message = "Login sucessful!",
                Data = CreateToken(user)
            };
            return response;
        }
        private static ServiceResponse<string> FailedLoginResponse()
        {
            var response = new ServiceResponse<string>();
            response.IsSuccess = false;
            response.Message = "Login failed.";

            return response;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private string CreateToken(User user)
        {
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("LastName", user.FullName),
                new Claim("Code", user.Code),
                new Claim("Email", user.Email),

            };

            string symmetricKey = _configuration.GetSection("TokenSettings:SymmetricKey").Value;
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(symmetricKey));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1000),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public Task<ServiceResponse<int>> RegisterAsync(User user, string password, int startUnitId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> ForgotPasswordAsync(InputModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed

            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var response = new ServiceResponse<string>()
            {
                IsSuccess = true,
                Message = "Sent sucessful!",
                Data = code
            };
            var sendMail = new SendmailRequest()
            {
                Displayname = "FA-Budget",
                ToDisplayname = user.FullName,
                Tomail = model.Email,
                Frommail = "FA-Budget-System@asahi-intecc.com",
                url = model.Code + response.Data,
                Data = "Forgot Password"
            };

            _sendmailServices.SendEmail(sendMail);
            return response;
        }

        public async Task<ServiceResponse<string>> ResetPasswordAsync(InputModel model)
        {
            var response = new ServiceResponse<string>();
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.IsSuccess = false;
                response.Data = "Không tìm thấy User";
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (result.Succeeded)
            {
                response.IsSuccess = true;
                response.Data = "OK";
            }

            return response;
        }
    }
}
