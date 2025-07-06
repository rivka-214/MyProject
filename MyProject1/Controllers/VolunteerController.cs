using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using Service.Services;
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
        private readonly IVolunteersCallLogic _volunteerCallService; // Add this field

        public VolunteerController(
            IService<VolunteersDto> service,
            IVolunteerLogic logic,
            IConfiguration config,
            IVolunteersCallLogic volunteerCallService) // Add this parameter
        {
            _service = service;
            _serviceLogic = logic;
            _config = config;
            _volunteerCallService = volunteerCallService; // Initialize the field
        }

        [HttpGet]
        public async Task<List<VolunteersDto>> Get() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<VolunteersDto> Get(int id) => await _service.GetByIdAsync(id);

       
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VolunteersDto value)
        {
            // בדיקה אם כבר קיים מתנדב עם אותו gmail
            var allVolunteers = await _service.GetAllAsync();
            if (allVolunteers.Any(v => v.Gmail == value.Gmail))
            {
                return BadRequest("כתובת המייל כבר רשומה במערכת.");
            }

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

        //[HttpGet("by-status")]
        //public async Task<IActionResult> GetCallsByStatus([FromQuery] string status)
        //{
        //    var calls = await _serviceLogic.GetCallsByStatus(status);
        //    return Ok(calls);
        //}

        [HttpGet("exists")]
        public async Task<IActionResult> CheckVolunteerExists([FromQuery] string gmail)
        {
            var volunteers = await _service.GetAllAsync();
            var exists = volunteers.Any(v => v.Gmail == gmail);
            return Ok(new { exists });
        }
        //כל הקריאות שהוקצו למתנדב
        [HttpGet("{volunteerId}/calls")]
        public async Task<ActionResult<List<CallsDto>>> GetCallsForVolunteer(int volunteerId)
        {
            var calls = await _volunteerCallService.GetAllCallsForVolunteer(volunteerId);
            if (calls == null || !calls.Any())
                return NotFound(new { error = "לא נמצאו קריאות למתנדב זה" });
            return Ok(calls);
        }
        //כל הקריאות למתנדב מסויים לפי סטטוס
        [HttpGet("{volunteerId}/calls/by-status/{status}")]
        public async Task<ActionResult<List<CallsDto>>> GetCallsForVolunteerByStatus(int volunteerId, string status)
        {
            var calls = await _volunteerCallService.GetCallsForVolunteerByStatus(volunteerId, status);
            if (!calls.Any())
                return NotFound(new { error = "לא נמצאו קריאות לסטטוס זה" });
            return Ok(calls);
        }
    }
}
