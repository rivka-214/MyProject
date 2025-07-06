using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Service.Services
{
    public class GeminiService : IGeminiService, IOpenAiService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public GeminiService(IConfiguration config)
        {
            _apiKey = config["Gemini:ApiKey"]
                      ?? throw new Exception("Missing Gemini API Key in configuration");

            _client = new HttpClient();

            _endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}";
        }

        public async Task<string> GetFirstAidInstructionsAsync(string description)
        {
            var prompt = $"אתה מתמחה בעזרה ראשונה. כתוב הוראות עזרה ראשונה למקרה הבא:\n{description}\nבקצרה ועם דגש על מה לעשות עכשיו.";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(_endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"API Error: {response.StatusCode} - {errorContent}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var contentElement) &&
                        contentElement.TryGetProperty("parts", out var parts) &&
                        parts.GetArrayLength() > 0)
                    {
                        var firstPart = parts[0];
                        if (firstPart.TryGetProperty("text", out var textElement))
                        {
                            return textElement.GetString() ?? "❗ לא נמצא טקסט בתשובה";
                        }
                    }
                }

                return "❗ לא התקבלה תשובה תקינה מ־Gemini";
            }
            catch (Exception ex)
            {
                return $"שגיאה בתקשורת עם Gemini: {ex.Message}";
            }
        }
    }
}
