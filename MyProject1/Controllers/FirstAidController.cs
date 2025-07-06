using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

[ApiController]
[Route("api/[controller]")]
public class FirstAidController : ControllerBase
{
    private readonly IFirstAidGuideService _guideService;
    private readonly IOpenAiService _openAiService;

    public FirstAidController(IFirstAidGuideService guideService, IOpenAiService openAiService)
    {
        _guideService = guideService;
        _openAiService = openAiService;
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
        var guides = _guideService.GetAll();
        return Ok(guides);
    }

    [HttpPost("ai")]
    public async Task<ActionResult<string>> GetAiFirstAidInstructions([FromBody] string description)
    {
        var result = await _openAiService.GetFirstAidInstructionsAsync(description);
        return Ok(result);
    }

}
