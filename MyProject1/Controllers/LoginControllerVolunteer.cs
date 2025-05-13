using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using BCrypt.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginControllerVolunteer : ControllerBase
    {
        private readonly IService<VolunteersDto> service;
        private readonly IConfiguration config;
        // GET: api/<LoginController>

        public LoginControllerVolunteer(IService<VolunteersDto> service, IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }
      //  [HttpGet]
     
        // GET api/<LoginController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<LoginController>
        //[HttpPost]
        //public UserDto Post([FromBody] UserDto value)
        //{
        //    // הצפנה של הסיסמה לפני השמירה
        //   // value.password = BCrypt.Net.BCrypt.HashPassword(value.password);

        //    return service.AddItem(value);
        //}


        [HttpPost("/VolunteerLogin")]
   
        public IActionResult Login([FromBody] VolunteerLogin value)
        {
            var volunteer = Authenticate(value);
            if (volunteer == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = Generate(volunteer);
            return Ok(token);
        }

        private string Generate(VolunteersDto Volunteer)
        {

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier,Volunteer.Password),
            new Claim(ClaimTypes.Email,Volunteer.Gmail),
          // new Claim(ClaimTypes.Name,Volunteer.FullName),
          //  new Claim(ClaimTypes.Role,volunteer.Role),
            //new Claim(ClaimTypes.GivenName,volunteer.Name)
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    
        private VolunteersDto Authenticate(VolunteerLogin value)
        {
            VolunteersDto volunteer = service.GetAll().FirstOrDefault(x => x.Gmail == value.Gmail&& x.Password==value.Password );
            if (volunteer != null)
                return volunteer;
            return null;
        }



    }
}
