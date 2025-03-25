using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
       
    }
}
