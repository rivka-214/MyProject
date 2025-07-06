using System;
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

        public OpenAiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            _apiKey = config["OpenAI:ApiKey"]
                      ?? throw new Exception("Missing OpenAI API Key in configuration");

            Console.WriteLine($"🔑 Loaded OpenAI API Key: {_apiKey.Substring(0, 8)}..."); // רק חלק מהמפתח להדפסה
        }

        public async Task<string> GetFirstAidInstructionsAsync(string description)
        {
            var url = "https://api.openai.com/v1/chat/completions";

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
            new {
                role = "user",
                content = $"אתה מתמחה בעזרה ראשונה. כתוב הוראות עזרה ראשונה למקרה הבא:\n{description}\nבקצרה ועם דגש על מה לעשות עכשיו."
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

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                return "בוצעו יותר מדי בקשות. נא להמתין ולנסות שוב.";

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return "שגיאת אימות: מפתח ה-API שגוי או חסר הרשאות.";

            if (!response.IsSuccessStatusCode)
            {
                // נסה לנתח את תוכן התגובה לקבלת פרטים מדויקים יותר
                try
                {
                    var errorDoc = JsonDocument.Parse(responseContent);
                    if (errorDoc.RootElement.TryGetProperty("error", out var error))
                    {
                        var code = error.GetProperty("code").GetString() ?? "קוד שגיאה לא ידוע";
                        return $"שגיאה מ-API: {error.GetProperty("message").GetString() ?? "שגיאה לא ידועה"} (קוד: {code})";
                    }
                }
                catch
                {
                    // אם לא מצליחים לנתח JSON
                    return $"שגיאה: קוד סטטוס {((int)response.StatusCode)} - {response.ReasonPhrase}";
                }
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(responseStream);

            var choice = doc.RootElement.GetProperty("choices")[0];
            var message = choice.GetProperty("message");
            var content = message.GetProperty("content").GetString();

            return content ?? "לא נמצאו הוראות";
        }
    }
    }
