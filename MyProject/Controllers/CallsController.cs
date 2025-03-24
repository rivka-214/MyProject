<<<<<<< HEAD
﻿ using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
=======
﻿using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
<<<<<<< HEAD
    public class CallsController : ControllerBase, ICallsController
    {
        private readonly IService<CallsDto> service;
        public CallsController(IService<CallsDto> servise)
        {
            this.servise = servise;
        }

        // GET: api/<CallsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CallsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CallsController>
        [HttpPost]
        public void Post([FromForm] CallsDto)
        {

            return service.AddItem(calls);
        }

        // PUT api/<CallsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CallsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        //פונקציה להעלעת תמונה צריכים להעביר אותה מיקום
        private void UploadImage(IFormFile file)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Images/", file.Name);
            using (var stream = new FileStream(path, FileMode.Create))
            {

                file.CopyTo(stream);
            }
        }
=======
    public class CallsController : ControllerBase
    {
      
        private readonly IService<CallsDto> service;
        public CallsController(IService<CallsDto> service)
        {
                this.service = service;
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
       
        public CallsDto Post([FromForm]CallsDto  call)
        {
           //23c UploadImage(call.ArrImage);
           
            return  service.AddItem(call);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CallsDto value)
        {
            service.UpdateItem(id, value);
        }
        //private void UploadImage(IFormFile file)
        //{
        //    //ניתוב לתמונה
        //    var path = Path.Combine(Environment.CurrentDirectory, "Images/", file.FileName);
        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {

        //        file.CopyTo(stream);
        //    }
        //}

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteItem(id);
        }
       
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3
    }
}
