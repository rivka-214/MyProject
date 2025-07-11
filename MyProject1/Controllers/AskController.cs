using AiFirstAidApi.Models;
using AiFirstAidApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AskController : ControllerBase
{
    private readonly OpenAiService _openAiService;
    private readonly FirstAidService _firstAidService;

    public AskController(OpenAiService openAiService, FirstAidService firstAidService)
    {
        _openAiService = openAiService;
        _firstAidService = firstAidService;
    }

    // 🔹 מענה מה-JSON המקומי בלבד
    [HttpPost("local")]
    public IActionResult GetFromLocal([FromBody] PromptRequest request)
    {
        var localResponse = _firstAidService.GetInstructionByPrompt(request.Prompt);

        if (!string.IsNullOrEmpty(localResponse))
        {
            return Ok(new { answer = localResponse });
        }

        return NotFound(new { error = "לא נמצאה תשובה מתאימה מקומית." });
    }

    // 🔹 מענה מ-AI בלבד
    [HttpPost("ai")]
    public async Task<IActionResult> GetFromAi([FromBody] PromptRequest request)
    {
        try
        {
            var aiResponse = await _openAiService.AskAsync(request.Prompt);
            return Ok(new { answer = aiResponse });
        }
        catch (HttpRequestException)
        {
            return StatusCode(500, new { error = "שגיאה בשליחה לשרת OpenAI" });
        }
    }
}
