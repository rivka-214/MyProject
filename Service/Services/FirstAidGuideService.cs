using Common.Dto;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace Service.Services
{
    public interface IFirstAidGuideService
    {
        Task<List<FirstAidGuide>> GetGuidesByTextAsync(string description);

        // ✅ הוספה:
        List<FirstAidGuide> GetAll();
    }

    public class FirstAidGuideService : IFirstAidGuideService
    {
        private readonly IWebHostEnvironment _env;
        private List<FirstAidGuide> _guides;

        public FirstAidGuideService(IWebHostEnvironment env)
        {
            _env = env;
            LoadGuides();
        }

        private void LoadGuides()
        {
            var path = Path.Combine(_env.WebRootPath, "data", "firstAidGuides.json");
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _guides = JsonSerializer.Deserialize<List<FirstAidGuide>>(json, options) ?? new();

            Console.WriteLine($"✅ Loaded {_guides.Count} guides from JSON");

        }

        public Task<List<FirstAidGuide>> GetGuidesByTextAsync(string description)
        {
            var desc = description.ToLower();

            var result = _guides
                .Where(g => g.Tags != null && g.Tags.Any(tag => desc.Contains(tag.ToLower())))
                .ToList();

            return Task.FromResult(result);
        }

        // ✅ מימוש של GetAll
        public List<FirstAidGuide> GetAll()
        {
            return _guides;
        }
    }
}
