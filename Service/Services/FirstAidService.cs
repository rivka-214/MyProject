using AiFirstAidApi.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace AiFirstAidApi.Services
{
    public class FirstAidService
    {
        private readonly List<FirstAidInstruction> _instructions;

        public FirstAidService(IWebHostEnvironment env)
        {
            try
            {
                var filePath = Path.Combine(env.WebRootPath ?? "", "data", "firstAidGuides.json");
                Console.WriteLine($"INFO: מנסה לטעון קובץ מ: {filePath}");

                if (string.IsNullOrWhiteSpace(env.WebRootPath))
                {
                    Console.WriteLine("ERROR: WebRootPath ריק או לא מוגדר.");
                    _instructions = new List<FirstAidInstruction>();
                    return;
                }

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"ERROR: הקובץ לא נמצא בנתיב: {filePath}");
                    _instructions = new List<FirstAidInstruction>();
                    return;
                }

                var json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("ERROR: הקובץ ריק או מכיל תוכן לא תקין.");
                    _instructions = new List<FirstAidInstruction>();
                    return;
                }

                // ✅ כאן הוספה האפשרות להתעלם מרישיות
                _instructions = JsonSerializer.Deserialize<List<FirstAidInstruction>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (_instructions == null)
                {
                    Console.WriteLine("ERROR: הניסיון לדה-סיריאליזציה החזיר null.");
                    _instructions = new List<FirstAidInstruction>();
                    return;
                }

                if (!_instructions.Any())
                {
                    Console.WriteLine("ERROR: הרשימה נטענה אך ריקה.");
                    return;
                }

                Console.WriteLine($"INFO: נטענו {_instructions.Count} הוראות עזרה ראשונה.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION בעת טעינת ההוראות: {ex.Message}");
                _instructions = new List<FirstAidInstruction>();
            }
        }

        public string GetInstructionByPrompt(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return null;

            prompt = prompt.ToLower().Trim();

            foreach (var instruction in _instructions)
            {
                if (instruction == null) continue;
                if (instruction.Tags == null) instruction.Tags = new List<string>();

                var promptWords = prompt.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var tag in instruction.Tags)
                {
                    if (string.IsNullOrEmpty(tag)) continue;
                    foreach (var word in promptWords)
                    {
                        if (tag.ToLower().Contains(word))
                        {
                            return $"{instruction.Title}: {instruction.Description}";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(instruction.Title))
                {
                    foreach (var word in promptWords)
                    {
                        if (instruction.Title.ToLower().Contains(word))
                        {
                            return $"{instruction.Title}: {instruction.Description}";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(instruction.Description))
                {
                    foreach (var word in promptWords)
                    {
                        if (instruction.Description.ToLower().Contains(word))
                        {
                            return $"{instruction.Title}: {instruction.Description}";
                        }
                    }
                }
            }

            return null; // לא נמצאה התאמה
        }
    }
}
