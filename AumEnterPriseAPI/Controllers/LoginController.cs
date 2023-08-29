using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AumEnterPriseAPI.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUserManager iUserManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IConfiguration config, IUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = config;
            iUserManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] UserLoginViewModel userData)
        {
            try
            {
                if (userData != null && userData.UserName != null && userData.Password != null)
                {
                    UserViewModel userViewModel = iUserManager.GetUserByName(userData.UserName, userData.Password);
                    if (userViewModel != null)
                    {
                        //create claims details based on the user information
                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", userViewModel.UserID),
                        new Claim("DisplayName", userViewModel.FullName),
                        new Claim("UserName", userViewModel.UserName),
                        new Claim("Email", userViewModel.EmailID),
                        new Claim(ClaimTypes.Role, userViewModel.UserType)
                    };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn);

                        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                        return Ok(accessToken);
                    }
                    else
                    {
                        return BadRequest("Invalid credentials");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while generating JWT token for username {userData.UserName}. Exception:\r\n{ex.Message}");
                throw;
            }
        }
    }
}
