using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersCallsController : ControllerBase
    {
        private readonly IService<VolunteerCallsDto> _service;
        private readonly IVolunteersCallLogic _volunteerCallLogic;

        public VolunteersCallsController(
            IService<VolunteerCallsDto> service,
            IVolunteersCallLogic volunteerCallLogic)
        {
            _service = service;
            _volunteerCallLogic = volunteerCallLogic;
        }

        [HttpGet]
        public async Task<List<VolunteerCallsDto>> Get() => await _service.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<VolunteerCallsDto> Get(int id) => await _service.GetByIdAsync(id);

        [HttpPost]
        public async Task<VolunteerCallsDto> Post([FromBody] VolunteerCallsDto value) =>
            await _service.AddItemAsync(value);

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] VolunteerCallsDto value)
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

        [HttpGet("active/{volunteerId}")]
        public async Task<ActionResult<List<VolunteerCallsDto>>> GetActiveCalls(int volunteerId)
        {
            var result = await _volunteerCallLogic.GetActiveCallsForVolunteer(volunteerId);
            return Ok(result);
        }

        [HttpGet("history/{volunteerId}")]
        public async Task<ActionResult<List<VolunteerCallsDto>>> GetHistoryCalls(int volunteerId)
        {
            var result = await _volunteerCallLogic.GetHistoryCallsForVolunteer(volunteerId);
            return Ok(result);
        }

        [HttpPost("respond")]
        public async Task<ActionResult> RespondToCall([FromBody] VolunteerResponseDto request)
        {
            await _volunteerCallLogic.RespondToCall(request.CallId, request.VolunteerId, request.Response);
            return Ok(new { message = "תגובה נשמרה בהצלחה" });
        }

        [HttpPut("{callId}/{volunteerId}/status")]
        public async Task<ActionResult> UpdateVolunteerStatus(int callId, int volunteerId, [FromBody] UpdateVolunteerStatusDto request)
        {
            await _volunteerCallLogic.UpdateVolunteerStatus(callId, volunteerId, request.Status, request.Summary);
            return Ok(new { message = "סטטוס עודכן בהצלחה" });
        }

        [HttpGet("{callId}/info")]
        public async Task<ActionResult<CallVolunteersInfoDto>> GetCallVolunteersInfo(int callId)
        {
            var statusMsg = await _volunteerCallLogic.GetCallVolunteersInfo(callId);
            return Ok(new CallVolunteersInfoDto
            {
                CallId = callId,
                StatusMessage = statusMsg
            });
        }
    }
}
