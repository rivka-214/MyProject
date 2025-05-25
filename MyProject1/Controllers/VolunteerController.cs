using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IService<VolunteersDto> service;
        private readonly IVolunteerLogic serviceLogic;
        public VolunteerController(IService<VolunteersDto> service,IVolunteerLogic logic)
        {
            this.service = service;
            this.serviceLogic = logic;
        }   
        // GET: api/<VolunteerController>
        [HttpGet]
        public List<VolunteersDto> Get()
        {
            return service.GetAll();
        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        public VolunteersDto Get(int id)
        {
            return service.GetById(id); 

        }
        [HttpGet("nearby")]
        public IActionResult GetNearby(double locationX, double locationY)
        {
            var result = serviceLogic.GetNearbyVolunteers(locationX, locationY);
            return Ok(result);
        }


        // POST api/<VolunteerController>
        [HttpPost]

        public async Task<VolunteersDto> Post([FromBody] VolunteersDto value)
        {
            return await serviceLogic.RegisterVolunteerWithLocation(value);

        }
     


        // PUT api/<VolunteerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] VolunteersDto value)
        {
            service.UpdateItem(id, value);
        }

        // DELETE api/<VolunteerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
