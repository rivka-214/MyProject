using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService<UserDto> service;
        private readonly IConfiguration config;
        // GET: api/<LoginController>

        public LoginController(IService<UserDto> service, IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LoginController>
        [HttpPost]
        public void Post([FromBody] UserDto value)
        {
            service.AddItem(value);
        }

        [HttpPost("/login")]
        public string Login([FromBody] UserDto value)
        {
            var user = Authenticate(value);
            var token = Generate(user);
            return token;
        }

        private string Generate(UserDto user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials=new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.FirstName),
                new Claim(ClaimTypes.SerialNumber,user.PhoneNumber),
                new Claim(ClaimTypes.Email,user.Gmail),

            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(365),
                signingCredentials: credentials

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDto Authenticate(UserDto value)
        {
            UserDto user= service.GetAll().FirstOrDefault(x => x.Id == value.Id && x.FirstName == value.FirstName);
            if(user!=null) 
                return user;
            return null;
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
