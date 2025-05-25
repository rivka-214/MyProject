using Microsoft.AspNetCore.Mvc;
using Common.Dto;
using Service.Services;

namespace MyProject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstAidController : ControllerBase
    {
        private readonly IFirstAidGuideService _guideService;

        public FirstAidController(IFirstAidGuideService guideService)
        {
            _guideService = guideService;
        }

        [HttpPost("suggest")]
        public async Task<ActionResult<List<FirstAidGuide>>> Suggest([FromBody] string description)
        {
            var guides = await _guideService.GetGuidesByTextAsync(description);
            return Ok(guides);
        }
        [HttpGet("all")]
        public ActionResult<List<FirstAidGuide>> All()
        {
            return Ok(_guideService.GetAll());
        }

    }
}
