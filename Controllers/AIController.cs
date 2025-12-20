using Microsoft.AspNetCore.Mvc;
using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Authorization;
using FitnessCenterApp.Services;

namespace FitnessCenterApp.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly StabilityAIService _stabilityService;
        private readonly GeminiAIService _geminiService;

        public AIController(StabilityAIService stabilityService, GeminiAIService geminiService)
        {
            _stabilityService = stabilityService;
            _geminiService = geminiService;
        }

        public IActionResult Index()
        {
            return View(new AIPlanViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan(AIPlanViewModel model)
        {
            bool isInfoEmpty = model.Age == null && model.Height == null && model.Weight == null && string.IsNullOrEmpty(model.Gender) && string.IsNullOrEmpty(model.BodyType) && string.IsNullOrEmpty(model.Goal) && string.IsNullOrEmpty(model.UserMessage);
            bool isImageEmpty = model.UserImage == null || model.UserImage.Length == 0;

            if (isInfoEmpty && isImageEmpty)
            {
                ModelState.AddModelError("", "Lütfen en az bir bilgi girin veya bir fotoğraf yükleyin.");
                return View("Index", model);
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            string ageStr = model.Age.HasValue ? model.Age.Value.ToString() : "belirtilmemiş";
            string heightStr = model.Height.HasValue ? model.Height.Value.ToString() + " cm" : "belirtilmemiş";
            string weightStr = model.Weight.HasValue ? model.Weight.Value.ToString() + " kg" : "belirtilmemiş";
            string genderStr = string.IsNullOrEmpty(model.Gender) ? "belirtilmemiş" : model.Gender;
            string bodyTypeStr = string.IsNullOrEmpty(model.BodyType) ? "belirtilmemiş" : model.BodyType;
            string goalStr = string.IsNullOrEmpty(model.Goal) ? "Genel Sağlık ve Fit Görünüm (Varsayılan)" : model.Goal;

            string photoContext = isImageEmpty ? "Kullanıcı fotoğraf yüklemedi." : "Kullanıcı bir vücut fotoğrafı yükledi, analizi buna göre yap.";

            try
            {
                string prompt = $@"
                    Sen profesyonel bir fitness koçusun. Aşağıdaki bilgilere sahip bir danışan için kişiye özel bir antrenman ve beslenme programı yaz.
                    Bazı bilgiler 'belirtilmemiş' olabilir. Bu durumda genel ama etkili tavsiyeler ver.
                    Fotoğraf yüklenmişse, görsel analize dayalı tahminlerde bulun (örneğin 'Fotoğrafınızdan anladığım kadarıyla...').

                    Cevabını HTML formatında ver (sadece <p>, <ul>, <li>, <strong>, <h3>, <div class='alert alert-info'> etiketlerini kullan). 
                    Başlıkları h3 ile, maddeleri ul ile yaz. Samimi ve motive edici bir dil kullan.

                    Kullanıcı Bilgileri:
                    - Yaş: {ageStr}
                    - Cinsiyet: {genderStr}
                    - Boy: {heightStr}
                    - Kilo: {weightStr}
                    - Vücut Tipi: {bodyTypeStr}
                    - Hedef: {goalStr}
                    - Özel Notu: {model.UserMessage ?? "Yok"}
                    - Fotoğraf Durumu: {photoContext}

                    Eğer boy ve kilo belirtilmişse BMI hesapla, belirtilmemişse bu kısmı atla.";

                model.AIResponse = await _geminiService.GenerateTextPlan(prompt);
            }
            catch (Exception ex)
            {
                model.AIResponse = "<div class='alert alert-warning'>Metin planı oluşturulurken bir hata oldu.</div>";
            }

            try
            {
                if (!isImageEmpty)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.UserImage!.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        model.OriginalImageBase64 = Convert.ToBase64String(imageBytes);
                    }

                    string genderPrompt = string.IsNullOrEmpty(model.Gender) ? "person" : (model.Gender == "Kadın" ? "female" : "male");
                    string agePrompt = model.Age.HasValue ? $"{model.Age} years old" : "adult";
                    string bodyPrompt = "fit, healthy body";

                    if (goalStr.Contains("Kilo Verme")) bodyPrompt = "slim, fit, athletic, toned abs, weight loss transformation";
                    else if (goalStr.Contains("Kas Kazanma")) bodyPrompt = "hyper muscular, bodybuilder, huge biceps, six pack, strong";

                    string fullPrompt = $"{bodyPrompt}, {genderPrompt}, {agePrompt}, realistic photo, 8k resolution, cinematic lighting";

                    var generatedBase64 = await _stabilityService.GenerateEditedImageAsync(model.UserImage!, fullPrompt);

                    if (!string.IsNullOrEmpty(generatedBase64))
                    {
                        model.GeneratedImageBase64 = generatedBase64;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Metin planı hazırlandı ancak AI Resim Servisi cevap vermedi.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Resim Hatası: " + ex.Message);
                ModelState.AddModelError("", "Resim işlenirken bir hata oluştu, ancak planınız hazır.");
            }

            return View("Index", model);
        }
    }
}