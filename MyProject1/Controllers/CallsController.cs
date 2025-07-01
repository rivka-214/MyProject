using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CallsController : ControllerBase
    {
        private readonly ICallService _callService;
        private readonly IService<CallsDto> _service;

        public CallsController(ICallService callService, IService<CallsDto> service)
        {
            _callService = callService;
            _service = service;
        }

        // GET: api/Calls
        [HttpGet]
        public async Task<ActionResult<List<CallsDto>>> Get()
        {
            try
            {
                var calls = await _callService.GetAllAsync();
                return Ok(calls);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/Calls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CallsDto>> Get(int id)
        {
            try
            {
                var call = await _callService.GetByIdAsync(id);
                if (call == null)
                    return NotFound(new { error = "קריאה לא נמצאה" });
                return Ok(call);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Calls
        [HttpPost]
        public async Task<ActionResult<CallsDto>> Post([FromForm] CallsDto call)
        {
            try
            {
                var savedCall = await _callService.AddCallAsync(call, Request.Form.Files.FirstOrDefault());
                return Ok(savedCall);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Calls/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CallsDto value)
        {
            try
            {
                var existing = await _callService.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { error = "קריאה לא נמצאה" });

                await _service.UpdateItemAsync(id, value);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/Calls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _callService.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(new { error = "קריאה לא נמצאה" });

                await _service.DeleteItemAsync(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/Calls/status/5
        [HttpGet("status/{id}")]
        public async Task<ActionResult<string>> GetCallStatus(int id)
        {
            try
            {
                var status = await _callService.GetCallStatusWithVolunteersInfo(id);
                return Ok(new { status });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/Calls/5/assign-nearby
        [HttpPost("{callId}/assign-nearby")]
        public async Task<ActionResult> AssignNearbyVolunteers(int callId, [FromQuery] double locationX, [FromQuery] double locationY)
        {
            try
            {
                await _callService.AssignNearbyVolunteersToCall(callId, locationX, locationY);
                return Ok(new { message = "מתנדבים הוקצו בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Calls/5/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] StatusDto statusDto)
        {
            try
            {
                await _callService.UpdateStatus(id, statusDto.Status);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Calls/5/complete
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Volunteer")]
        public async Task<ActionResult> CompleteCall(int id, [FromBody] CompleteCallDto dto)
        {
            try
            {
                var volunteerId = int.Parse(User.FindFirst("id")?.Value);
                await _callService.CompleteCall(id, dto, volunteerId);
                return Ok(new { message = "הקריאה עודכנה בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("by-user")]
        [Authorize]
        public async Task<ActionResult<List<CallsDto>>> GetCallsByUser()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"UserId from token: {userIdStr}");

            if (!int.TryParse(userIdStr, out int userId))
            {
                Console.WriteLine("Invalid UserId in token");
                return Unauthorized();
            }

            var calls = await _callService.GetCallsByUserId(userId);
            Console.WriteLine($"Found {calls.Count} calls for user.");

            return Ok(calls);
        }
    }
}