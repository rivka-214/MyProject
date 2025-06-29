
using Common.Dto;
using Microsoft.AspNetCore.Authorization;
﻿ using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;
using System.Runtime.CompilerServices;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

  
    public class CallsController : ControllerBase
    {
      
        private readonly IService<CallsDto> service;
       
        private readonly IVolunteersCallLogic logic;
        private readonly ICallService callService;

        public CallsController(IService<CallsDto> service, IVolunteersCallLogic logic,ICallService callService)
        {
            this.service = service;
            this.logic = logic;
            this.callService = callService;
        }

        // GET: api/<CategoryController>      
        [HttpGet] 
        [Authorize]
        public async Task<List<CallsDto>> Get()
        {
            return await service.GetAllAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<CallsDto> Get(int id)
        {
            return await service.GetByIdAsync(id);
        }


        [HttpPost]
        [Authorize]
      
        public async Task<CallsDto> Post([FromForm] CallsDto call)
        {
            Console.WriteLine($"מיקום שהתקבל: X={call.LocationX}, Y={call.LocationY}");

            if (call.FileImage != null)
                await UploadImage(call.FileImage);

            call.Status = "נפתחה";
            var savedCall = await service.AddItemAsync(call);

            if (call.LocationX != 0 && call.LocationY != 0)
            {
                await logic.AssignNearbyVolunteersToCall(savedCall.Id, call.LocationX, call.LocationY);
            }

            return savedCall;
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task Put(int id, [FromBody] CallsDto value)
        {
            await Task.Run(() => service.UpdateItemAsync(id, value));
        }

        private async Task UploadImage(IFormFile file)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Images\\", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task Delete(int id)
        {
            await Task.Run(() => service.DeleteItemAsync(id));
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetCallStatus(int id)
        {
            var call = await Task.FromResult(service.GetByIdAsync(id));
            if (call == null)
                return NotFound(new { error = "קריאה לא נמצאה" });

            return Ok(new { status = call.Status });
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
            await Task.Run(() => callService.UpdateStatus(id, statusDto.Status));
            return Ok();
        }

        [HttpPut("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> CompleteCall(int id, [FromBody] CompleteCallDto dto)
        {
            await Task.Run(() => callService.CompleteCall(id, dto));
            return Ok("הקריאה עודכנה בהצלחה");
        }
    }
}

