using AutoMapper.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace Service.Services
{
    public interface IOpenAiService
    {
        Task<string> GetFirstAidInstructionsAsync(string description);
    }

    public class OpenAiService : IOpenAiService
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public OpenAiService(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _httpClient = httpClient;

            // קריאה ישירה מתוך קובץ הקונפיגורציה
            _apiKey = config["OpenAI:ApiKey"]
                      ?? throw new Exception("Missing OpenAI API Key in configuration");
        }

        public async Task<string> GetFirstAidInstructionsAsync(string description)
        {
            try
            {
                var url = "https://api.openai.com/v1/chat/completions";

                var requestBody = new
                {
                    model = "gpt-3.5-turbo", // שים לב לשינוי כאן
                    messages = new[]
                    {
                new {
                    role = "user",
                    content = $"אתה מתמחה בעזרה ראשונה. כתוב הוראות עזרה ראשונה מדויקות וברורות למקרה הבא:\n{description}\nבקצרה ועם דגש על מה לעשות עכשיו."
                }
            },
                    max_tokens = 500,
                    temperature = 0.3
                };

                var jsonBody = JsonSerializer.Serialize(requestBody);
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(httpRequest);

                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(responseStream);

                var choice = doc.RootElement.GetProperty("choices")[0];
                var message = choice.GetProperty("message");
                var content = message.GetProperty("content").GetString();

                return content ?? "לא נמצאו הוראות";
            }
            catch (Exception ex)
            {
                return $"שגיאה בקריאת עזרה ראשונה: {ex.Message}";
            }
        }

    }
}
