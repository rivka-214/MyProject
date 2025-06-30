using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private readonly IService<CallsDto> service;
        private readonly IVolunteersCallLogic logic;
        private readonly ICallService callService;

        public CallsController(IService<CallsDto> service, IVolunteersCallLogic logic, ICallService callService)
        {
            this.service = service;
            this.logic = logic;
            this.callService = callService;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<CallsDto>> Get() => await service.GetAllAsync();

        [HttpGet("{id}")]
        [Authorize]
        public async Task<CallsDto> Get(int id) => await service.GetByIdAsync(id);

        [HttpPost]
        [Authorize]
        public async Task<CallsDto> Post([FromForm] CallsDto call)
        {
            return await callService.CreateCallAsync(call);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task Put(int id, [FromBody] CallsDto value)
        {
            await service.UpdateItemAsync(id, value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task Delete(int id)
        {
            await service.DeleteItemAsync(id);
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetCallStatus(int id)
        {
            var status = await callService.GetStatus(id);
            if (status == null)
                return NotFound(new { error = "קריאה לא נמצאה" });

            return Ok(new { status });
        }

        [HttpPost("{callId}/assign-nearby")]
        [Authorize]
        public async Task<IActionResult> AssignNearbyVolunteers(int callId, [FromQuery] double locationX, [FromQuery] double locationY)
        {
            await logic.AssignNearbyVolunteersToCall(callId, locationX, locationY);
            return Ok("מתנדבים הוקצו בהצלחה");
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusDto statusDto)
        {
            await callService.UpdateStatus(id, statusDto.Status);
            return Ok();
        }

        [HttpPut("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> CompleteCall(int id, [FromBody] CompleteCallDto dto)
        {
            await callService.CompleteCall(id, dto);
            return Ok("הקריאה עודכנה בהצלחה");
        }
    }
}
