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
        private readonly IService<VolunteerCallsDto> service;
        private readonly IVolunteersCallLogic volunteerCallService;
        private readonly IService<VolunteersDto> volunteerService;
        private readonly IService<CallsDto> callsService;

        public VolunteersCallsController(
            IService<VolunteerCallsDto> service,
            IVolunteersCallLogic volunteerCallService,
            IService<VolunteersDto> volunteerService,
            IService<CallsDto> callsService)
        {
            this.service = service;
            this.volunteerCallService = volunteerCallService;
            this.volunteerService = volunteerService;
            this.callsService = callsService;
        }

        [HttpGet]
        public async Task<List<VolunteerCallsDto>> Get()
        {
            return await service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<VolunteerCallsDto> Get(int id)
        {
            return await service.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<VolunteerCallsDto> Post([FromBody] VolunteerCallsDto value)
        {
            return await service.AddItemAsync(value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] VolunteerCallsDto value)
        {
            await service.UpdateItemAsync(id, value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteItemAsync(id);
            return NoContent();
        }

        [HttpGet("active/{volunteerId}")]
        public async Task<ActionResult<List<VolunteerCallsDto>>> GetActiveCalls(int volunteerId)
        {
            try
            {
                var activeCalls = await volunteerCallService.GetActiveCallsForVolunteer(volunteerId);
                return Ok(activeCalls);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בקבלת קריאות פעילות: {ex.Message}");
            }
        }

        [HttpGet("history/{volunteerId}")]
        public async Task<ActionResult<List<VolunteerCallsDto>>> GetHistoryCalls(int volunteerId)
        {
            try
            {
                var historyCalls = await volunteerCallService.GetHistoryCallsForVolunteer(volunteerId);
                return Ok(historyCalls);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בקבלת היסטוריית קריאות: {ex.Message}");
            }
        }

        [HttpPost("respond")]
        public async Task<ActionResult> RespondToCall([FromBody] VolunteerResponseDto request)
        {
            try
            {
                await volunteerCallService.RespondToCall(request.CallId, request.VolunteerId, request.Response);
                return Ok(new { message = "תגובה נשמרה בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בשמירת תגובה: {ex.Message}");
            }
        }

        [HttpPut("{callId}/{volunteerId}/status")]
        public async Task<ActionResult> UpdateVolunteerStatus(int callId, int volunteerId, [FromBody] UpdateVolunteerStatusDto request)
        {
            try
            {
                await volunteerCallService.UpdateVolunteerStatus(callId, volunteerId, request.Status, request.Summary);
                return Ok(new { message = "סטטוס עודכן בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בעדכון סטטוס: {ex.Message}");
            }
        }

        [HttpGet("{callId}/info")]
        public async Task<ActionResult<CallVolunteersInfoDto>> GetCallVolunteersInfo(int callId)
        {
            try
            {
                var info = await volunteerCallService.GetCallVolunteersInfo(callId);
                return Ok(new CallVolunteersInfoDto
                {
                    CallId = callId,
                    StatusMessage = info
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בקבלת מידע על קריאה: {ex.Message}");
            }
        }
    }
}
