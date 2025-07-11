using AiFirstAidApi.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class OpenAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _model = "gpt-4";

    public OpenAiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        // וודא שה-BaseUrl מסתיים ב- "/" כדי שהקונקטור יעבוד טוב
        var baseUrl = configuration["OpenAI:BaseUrl"];
        if (!baseUrl.EndsWith("/"))
            baseUrl += "/";

        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", configuration["OpenAI:ApiKey"]);
    }

    public async Task<string> AskAsync(string prompt)
    {
        var request = new ChatCompletionRequest
        {
            Model = _model,
            Messages = new List<ChatMessage>
            {
                new ChatMessage { Role = "user", Content = prompt }
            }
        };

        // נסה לקרוא לנתיב בלי ה-openai/v1 כי זה כנראה כלול ב-BaseUrl
        var response = await _httpClient.PostAsJsonAsync("chat/completions", request);

        response.EnsureSuccessStatusCode();

        var completion = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>();

        return completion?.Choices?.FirstOrDefault()?.Message?.Content ?? "לא התקבלה תגובה";
    }
}
