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
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return Unauthorized("User is not authenticated");

            var claims = identity.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var gmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            if (userId == null || gmail == null || role == null)
                return BadRequest("Missing claims");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var newClaims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Email, gmail),
        new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role)
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: newClaims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            string newJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = newJwt });
        }


        private string Generate(VolunteersDto volunteer)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, volunteer.Id.ToString()), // מזהה מתנדב
        new Claim(ClaimTypes.Email, volunteer.Gmail),
        new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Volunteer") // ✅ תפקיד ברור
    };

            var token = new JwtSecurityToken(
      issuer: config["Jwt:Issuer"],
      audience: config["Jwt:Audience"],
      claims: claims,
      expires: DateTime.UtcNow.AddDays(7), // ✅ חובה! אחרת exp לא ייכנס לטוקן
      signingCredentials: credentials
  );

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
