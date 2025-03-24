 using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
