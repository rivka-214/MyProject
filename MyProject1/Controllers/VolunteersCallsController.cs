using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersCallsController : ControllerBase
    {
        private readonly IService<VolunteerCallsDto> service;
        private readonly IVolunteersCallLogic volunteerCallService;

        public VolunteersCallsController(IService<VolunteerCallsDto> service, IVolunteersCallLogic volunteerCallService)
        {
            this.service = service;
            this.volunteerCallService = volunteerCallService;
        }
        // GET: api/<VolunteersCallsController>
        [HttpGet]
        public List<VolunteerCallsDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        public VolunteerCallsDto Get(int id)
        {
            return service.GetById(id);

        }

        // POST api/<VolunteerController>
        [HttpPost]
        public VolunteerCallsDto Post([FromBody] VolunteerCallsDto value)
        {
            return service.AddItem(value);
        }

        // PUT api/<VolunteerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] VolunteerCallsDto value)
        {
            service.UpdateItem(id, value);
        }

        // DELETE api/<VolunteerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
        [HttpPost("assign")]

        public async Task<IActionResult> AssignNearby([FromBody] AssignRequestDto dto)
        {
            var logic = service as VolunteersCallService;
            if (logic == null)
                return BadRequest("שירות לא תקין");

            await logic.AssignNearbyVolunteersToCall(dto.CallId, dto.LocationX, dto.LocationY);
            return Ok();
        }

        /// GET /api/VolunteerCalls/active/{volunteerId}
        /// מחזיר קריאות פעילות למתנדב

        [HttpGet("active/{volunteerId}")]
        public ActionResult<List<VolunteerCallsDto>> GetActiveCalls(int volunteerId)
        {
            try
            {
                var activeCalls = volunteerCallService.GetActiveCallsForVolunteer(volunteerId);
                return Ok(activeCalls);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בקבלת קריאות פעילות: {ex.Message}");
            }
        }

        /// <summary>
        /// GET /api/VolunteerCalls/history/{volunteerId}
        /// מחזיר היסטוריית קריאות למתנדב
        /// </summary>
        [HttpGet("history/{volunteerId}")]
        public ActionResult<List<VolunteerCallsDto>> GetHistoryCalls(int volunteerId)
        {
            try
            {
                var historyCalls = volunteerCallService.GetHistoryCallsForVolunteer(volunteerId);
                return Ok(historyCalls);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בקבלת היסטוריית קריאות: {ex.Message}");
            }
        }

        /// <summary>
        /// POST /api/VolunteerCalls/respond
        /// מתנדב מגיב לקריאה (going/cant)
        /// </summary>
        [HttpPost("respond")]
        public ActionResult RespondToCall([FromBody] VolunteerResponseDto request)
        {
            try
            {
                volunteerCallService.RespondToCall(request.CallId, request.VolunteerId, request.Response);
                return Ok(new { message = "תגובה נשמרה בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בשמירת תגובה: {ex.Message}");
            }
        }

        /// <summary>
        /// PUT /api/VolunteerCalls/{callId}/{volunteerId}/status
        /// עדכון סטטוס מתנדב לקריאה ספציפית
        /// </summary>
        [HttpPut("{callId}/{volunteerId}/status")]
        public ActionResult UpdateVolunteerStatus(int callId, int volunteerId, [FromBody] UpdateVolunteerStatusDto request)
        {
            try
            {
                volunteerCallService.UpdateVolunteerStatus(callId, volunteerId, request.Status, request.Summary);
                return Ok(new { message = "סטטוס עודכן בהצלחה" });
            }
            catch (System.Exception ex)
            {
                return BadRequest($"שגיאה בעדכון סטטוס: {ex.Message}");
            }
        }

        /// <summary>
        /// GET /api/VolunteerCalls/{callId}/info
        /// מידע על מתנדבים שיצאו לקריאה
        /// </summary>
        [HttpGet("{callId}/info")]
        public ActionResult<CallVolunteersInfoDto> GetCallVolunteersInfo(int callId)
        {
            try
            {
                var info = volunteerCallService.GetCallVolunteersInfo(callId);
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



