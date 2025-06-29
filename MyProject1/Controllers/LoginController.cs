using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService<UserDto> service;
        private readonly IConfiguration config;

        public LoginController(IService<UserDto> service, IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }

        // רישום משתמש חדש
        [HttpPost]
        public async Task<UserDto> Post([FromBody] UserDto value)
        {
            return await service.AddItemAsync(value);
        }

        // התחברות משתמש
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserLogin value)
        {
            var user = await Authenticate(value);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = Generate(user);
            return Ok(new
            {
                token,
                role = user.Role ?? "User"
            });
        }

        // יצירת טוקן JWT
        private string Generate(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Gmail),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddYears(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // אימות משתמש
        private async Task<UserDto?> Authenticate(UserLogin value)
        {
            var users = await service.GetAllAsync();
            return users.FirstOrDefault(x => x.password == value.password && x.Gmail == value.Gmail);
        }
    }
}
