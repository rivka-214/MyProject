using Common.Dto;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IService<VolunteersDto> _service;
        private readonly IVolunteerLogic _serviceLogic;
        private readonly IConfiguration _config;

        public VolunteerController(IService<VolunteersDto> service, IVolunteerLogic logic, IConfiguration config)
        {
            _service = service;
            _serviceLogic = logic;
            _config = config;
        }

        [HttpGet]
        public async Task<List<VolunteersDto>> Get() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<VolunteersDto> Get(int id) => await _service.GetByIdAsync(id);

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby(double locationX, double locationY)
        {
            var result = await _serviceLogic.GetNearbyVolunteers(locationX, locationY);
            return Ok(result);
        }

        [HttpGet("nearby-alerts")]
        public async Task<IActionResult> GetNearbyAlerts([FromQuery] int id)
        {
            var volunteer = await _service.GetByIdAsync(id);
            if (volunteer == null)
                return NotFound("מתנדב לא נמצא");

            if (volunteer.LocationX == null || volunteer.LocationY == null)
                return BadRequest("למתנדב אין מיקום");

            var calls = await _serviceLogic.GetNearbyOpenCalls(volunteer.LocationX.Value, volunteer.LocationY.Value);
            return Ok(calls);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VolunteersDto value)
        {
            var createdVolunteer = await _serviceLogic.RegisterVolunteerWithLocation(value);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, createdVolunteer.Gmail),
                new Claim(ClaimTypes.NameIdentifier, createdVolunteer.Id.ToString()),
                new Claim(ClaimTypes.Role, "Volunteer")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
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
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> Put(int id, [FromBody] VolunteersDto value)
        {
            await _service.UpdateItemAsync(id, value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteItemAsync(id);
            return NoContent();
        }

        [HttpGet("by-status")]
        public async Task<IActionResult> GetCallsByStatus([FromQuery] string status)
        {
            var calls = await _serviceLogic.GetCallsByStatus(status);
            return Ok(calls);
        }

        [HttpGet("exists")]
        public async Task<IActionResult> CheckVolunteerExists([FromQuery] string gmail)
        {
            var volunteers = await _service.GetAllAsync();
            var exists = volunteers.Any(v => v.Gmail == gmail);
            return Ok(new { exists });
        }
    }
}
