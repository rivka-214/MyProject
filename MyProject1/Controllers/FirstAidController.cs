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
        public async Task<ActionResult<List<FirstAidGuide>>> Suggest([FromBody] FirstAidRequestDto request)
        {
            var guides = await _guideService.GetGuidesByTextAsync(request.Description);
            return Ok(guides);
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<FirstAidGuide>>> All()
        {
            var guides = await Task.FromResult(_guideService.GetAll());
            return Ok(guides);
        }
    }
}
