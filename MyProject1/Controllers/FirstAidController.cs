using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Services;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FirstAidController : ControllerBase
    {
        private readonly OpenAiService _openAiService;

        public FirstAidController(OpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        [HttpPost("ai")]
        public async Task<IActionResult> GetFirstAidInstructions([FromBody] string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return BadRequest("Description is required");

            var result = await _openAiService.GetFirstAidInstructionsAsync(description);
            return Ok(result);
        }
    }
}
