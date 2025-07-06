using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyProject.Services
{
    public class FirstAidAiService
    {
        private readonly HttpClient _httpClient;

        public FirstAidAiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetFirstAidInstructionsAsync(string prompt)
        {
            var requestBody = new
            {
                model = "mistral",
                prompt = prompt,
                stream = false
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            return doc.RootElement.GetProperty("response").GetString();
        }
    }
}
