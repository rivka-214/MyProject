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
    public class VolunteerController : ControllerBase
    {
        private readonly IService<VolunteersDto> service;
        private readonly IVolunteerLogic serviceLogic;
        private readonly IConfiguration config;

        public VolunteerController(IService<VolunteersDto> service, IVolunteerLogic logic, IConfiguration config)
        {
            this.service = service;
            this.serviceLogic = logic;
            this.config = config;
        }

        [HttpGet]
        public List<VolunteersDto> Get()
        {
            return service.GetAll();
        }

        [HttpGet("{id}")]
        public VolunteersDto Get(int id)
        {
            return service.GetById(id);
        }

        [HttpGet("nearby")]
        public IActionResult GetNearby(double locationX, double locationY)
        {
            var result = serviceLogic.GetNearbyVolunteers(locationX, locationY);
            return Ok(result);
        }

        // ✅ נקודת API חדשה – מחזירה קריאות שקרובות למתנדב
        [HttpGet("nearby-alerts")]
        public IActionResult GetNearbyCallsForVolunteer(int id)
        {
            var volunteer = service.GetById(id);
            if (volunteer == null)
                return NotFound("Volunteer not found");

            if (volunteer.LocationX == null || volunteer.LocationY == null)
                return BadRequest("למתנדב אין מיקום");

            var calls = serviceLogic.GetNearbyOpenCalls(
                volunteer.LocationX.Value,
                volunteer.LocationY.Value);

            return Ok(calls);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VolunteersDto value)
        {
            var createdVolunteer = await serviceLogic.RegisterVolunteerWithLocation(value);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, createdVolunteer.Gmail),
                new Claim(ClaimTypes.NameIdentifier, createdVolunteer.Id.ToString()),
                new Claim(ClaimTypes.Role, "Volunteer")
            };

            var token = new JwtSecurityToken(
               issuer: config["Jwt:Issuer"],
               audience: config["Jwt:Audience"],
               claims: claims,
               signingCredentials: credentials
           );
 
      
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwtToken,
                role = "Volunteer"
            });
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] VolunteersDto value)
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
