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
        public List<UserDto> Get()
        {
            return service.GetAll();
        }

        [HttpGet("{id}")]
        public UserDto Get(int id)
        {
            return service.GetById(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDto user)
        {
            if (string.IsNullOrEmpty(user.Role))
                user.Role = "User";

            var createdUser = service.AddItem(user);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Email, createdUser.Gmail),
        new Claim(ClaimTypes.NameIdentifier, createdUser.Id.ToString()),
        new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", createdUser.Role ?? "User")
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
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
        public void Put(int id, [FromBody] UserDto value)
        {
            service.UpdateItem(id, value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
    }
}
