
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
        public List<CallsDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public CallsDto Get(int id)
        {
            return service.GetById(id);
        }

        // POST api/<CategoryController>
        [HttpPost]

        [HttpPost]
        public CallsDto Post([FromForm] CallsDto call)
        {
            Console.WriteLine($"מיקום שהתקבל: X={call.LocationX}, Y={call.LocationY}");

            if (call.FileImage != null)
                UploadImage(call.FileImage);

            return service.AddItem(call);
        }


        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CallsDto value)
        {
            service.UpdateItem(id, value);
        }
        private void UploadImage(IFormFile file)
        {
            //ניתוב לתמונה
            var path = Path.Combine(Environment.CurrentDirectory, "Images\\", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {

                file.CopyTo(stream);
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
        [HttpGet("status/{id}")]
        public IActionResult GetCallStatus(int id)
        {
            var call = service.GetById(id);
            if (call == null)
                return NotFound(new { error = "קריאה לא נמצאה" });

            return Ok(new { status = call.Status });
        }
        [HttpPost("{callId}/assign-nearby")]
       
        public async Task<IActionResult> AssignNearbyVolunteers(int callId, [FromQuery] double locationX, [FromQuery] double locationY)
        {
            await logic.AssignNearbyVolunteersToCall(callId, locationX, locationY);
            return Ok("מתנדבים הוקצו בהצלחה");
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] StatusDto statusDto)
        {
            callService.UpdateStatus(id, statusDto.Status);
            return Ok();
        }
        [HttpPut("{id}/complete")]
        public IActionResult CompleteCall(int id, [FromBody] CompleteCallDto dto)
        {
            callService.CompleteCall(id, dto);
            return Ok("הקריאה עודכנה בהצלחה");
        }


    }
}
