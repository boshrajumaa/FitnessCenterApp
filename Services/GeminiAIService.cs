using Google.GenAI;
using System.Threading.Tasks;

namespace FitnessCenterApp.Services
{
    public class GeminiAIService
    {
        private readonly string _apiKey = "AIzaSyCsmzPQVyd4Ylc8Q-a6NYah431GgYl-Wmg";

        public async Task<string> GenerateTextPlan(string prompt)
        {
            try
            {
                var client = new Google.GenAI.Client(apiKey: _apiKey);

                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash",
                    contents: prompt
                );

                string resultText = response.Candidates[0].Content.Parts[0].Text;

                return resultText ?? "AI boş cevap döndürdü.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GOOGLE SDK HATASI: {ex.Message}");
                return $"AI Servis Hatası: {ex.Message}. Lütfen internet bağlantınızı kontrol edin.";
            }
        }
    }
}