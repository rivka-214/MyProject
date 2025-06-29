using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Common.Dto;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IService<UserDto> service;
        private readonly IConfiguration config;

        public UserController(IService<UserDto> service, IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }

        [HttpGet]
        public async Task<List<UserDto>> Get()
        {
            return await service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<UserDto> Get(int id)
        {
            return await service.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto user)
        {
            if (string.IsNullOrEmpty(user.Role))
                user.Role = "User";

            var createdUser = await service.AddItemAsync(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, createdUser.Gmail),
                new Claim(ClaimTypes.NameIdentifier, createdUser.Id.ToString()),
                new Claim(ClaimTypes.Role, createdUser.Role ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: credentials
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwtToken,
                role = createdUser.Role
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto value)
        {
            await service.UpdateItemAsync(id, value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
