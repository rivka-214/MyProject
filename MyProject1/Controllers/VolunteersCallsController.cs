using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Volunteer")]
    public class VolunteersCallsController : ControllerBase
    {
        private readonly IService<VolunteerCallsDto> _service;
        private readonly IVolunteersCallLogic _volunteerCallService;

        public VolunteersCallsController(
            IService<VolunteerCallsDto> service,
            IVolunteersCallLogic volunteerCallService)
        {
            _service = service;
            _volunteerCallService = volunteerCallService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VolunteerCallsDto>>> Get()
        {
            var calls = await _service.GetAllAsync();
            return Ok(calls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VolunteerCallsDto>> Get(int id)
        {
            var call = await _service.GetByIdAsync(id);
            if (call == null)
                return NotFound(new { error = "קריאה לא נמצאה" });
            return Ok(call);
        }

        [HttpPost]
        public async Task<ActionResult<VolunteerCallsDto>> Post([FromBody] VolunteerCallsDto value)
        {
            try
            {
                var created = await _service.AddItemAsync(value);
                return Ok(created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] VolunteerCallsDto value)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var existing = await _service.GetByIdAsync(id);
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

        [HttpGet("active/{volunteerId}")]
        public async Task<ActionResult<List<VolunteerCallsDto>>> GetActiveCalls(int volunteerId)
        {
            try
            {
                var activeCalls = await _volunteerCallService.GetActiveCallsForVolunteer(volunteerId);
                return Ok(activeCalls);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("history/{volunteerId}")]
        public async Task<ActionResult<List<VolunteerCallsDto>>> GetHistoryCalls(int volunteerId)
        {
            try
            {
                var historyCalls = await _volunteerCallService.GetHistoryCallsForVolunteer(volunteerId);
                return Ok(historyCalls);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("respond")]
        public async Task<ActionResult> RespondToCall([FromBody] VolunteerResponseDto request)
        {
            try
            {
                var currentVolunteerId = int.Parse(User.FindFirst("id")?.Value);
                await _volunteerCallService.RespondToCall(request.CallId, request.VolunteerId, request.Response, currentVolunteerId);
                return Ok(new { message = "תגובה נשמרה בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{callId}/{volunteerId}/status")]
        public async Task<ActionResult> UpdateVolunteerStatus(int callId, int volunteerId, [FromBody] UpdateVolunteerStatusDto request)
        {
            try
            {
                var currentVolunteerId = int.Parse(User.FindFirst("id")?.Value);
                await _volunteerCallService.UpdateVolunteerStatus(callId, volunteerId, request.Status, currentVolunteerId, request.Summary);
                return Ok(new { message = "סטטוס עודכן בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{callId}/info")]
        public async Task<ActionResult<CallVolunteersInfoDto>> GetCallVolunteersInfo(int callId)
        {
            try
            {
                var info = await _volunteerCallService.GetCallVolunteersInfo(callId);
                return Ok(info);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}