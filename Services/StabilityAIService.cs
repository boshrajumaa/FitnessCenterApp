using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FitnessCenterApp.Services
{
    public class StabilityAIService
    {
        private readonly string _apiKey = "sk-sfAAQTneo343w2GUAJrp9GZCiSb68EgQOG0m0tftCJAXkqY4";
        private readonly string _engineId = "stable-diffusion-xl-1024-v1-0"; // En kaliteli motor
        private readonly string _apiUrl;

        public StabilityAIService()
        {
            _apiUrl = $"https://api.stability.ai/v1/generation/{_engineId}/image-to-image";
        }

        public async Task<string?> GenerateEditedImageAsync(IFormFile originalImage, string prompt)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var memoryStream = new MemoryStream())
                    {
                        await originalImage.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        using (var content = new MultipartFormDataContent())
                        {
                            var imageContent = new ByteArrayContent(imageBytes);
                            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(originalImage.ContentType);
                            content.Add(imageContent, "init_image", originalImage.FileName);

                            content.Add(new StringContent("IMAGE_STRENGTH"), "init_image_mode");
                            content.Add(new StringContent("0.35"), "image_strength");
                            content.Add(new StringContent(prompt), "text_prompts[0][text]");
                            content.Add(new StringContent("1"), "text_prompts[0][weight]");
                            content.Add(new StringContent("ugly, fat, blurry, low quality, distorted"), "text_prompts[1][text]");
                            content.Add(new StringContent("-1"), "text_prompts[1][weight]");

                            var response = await client.PostAsync(_apiUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                var jsonString = await response.Content.ReadAsStringAsync();
                                var jsonNode = JsonNode.Parse(jsonString);
                                return jsonNode?["artifacts"]?[0]?["base64"]?.ToString();
                            }
                            else
                            {
                                var error = await response.Content.ReadAsStringAsync();
                                System.Diagnostics.Debug.WriteLine($"API HATASI: {error}");
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KRİTİK HATA: {ex.Message}");
                return null;
            }
        }
    }
}