using Microsoft.AspNetCore.Mvc;
using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenterApp.Controllers
{
    [Authorize] // Sadece üyeler kullanabilsin
    public class AIController : Controller
    {
        // GET: Form sayfasını gösterir
        public IActionResult Index()
        {
            return View();
        }

        // POST: Formu işler ve sonuç üretir
        [HttpPost]
        public IActionResult GeneratePlan(AIPlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // --- YAPAY ZEKA SİMÜLASYONU (MOCK AI) ---
            // Gerçek API anahtarı olmadan projenin çalışması için bu simülasyonu kullanıyoruz.
            // OpenAI API entegrasyonu kodları aşağıda yorum satırı olarak verilmiştir.

            string plan = "";

            // Basit bir kural tabanlı yapay zeka mantığı
            if (model.Goal == "Kilo Verme")
            {
                plan = $@"
                <h4>🏃‍♂️ Kilo Verme Programınız Hazır!</h4>
                <p>Boyunuz ({model.Height} cm) ve kilonuz ({model.Weight} kg) analiz edildi.</p>
                <ul>
                    <li><strong>Sabah:</strong> 30 dk aç karnına tempolu yürüyüş.</li>
                    <li><strong>Öğle:</strong> Protein ağırlıklı salata.</li>
                    <li><strong>Akşam:</strong> Sebze yemeği ve yoğurt.</li>
                    <li><strong>Egzersiz:</strong> Haftada 4 gün Kardiyo + HIIT antrenmanı.</li>
                </ul>
                <div class='alert alert-info'>💡 Tavsiye: Günde en az 2.5 litre su içmeyi unutmayın!</div>";
            }
            else if (model.Goal == "Kas Kazanma")
            {
                plan = $@"
                <h4>💪 Kas Kazanma Programınız Hazır!</h4>
                <p>Vücut kitle indeksinize göre güçlü bir antrenman planı:</p>
                <ul>
                    <li><strong>Beslenme:</strong> Günlük protein alımınızı artırın (Kilonuz x 2g).</li>
                    <li><strong>Antrenman:</strong> Ağırlık antrenmanlarına odaklanın (Hypertrophy).</li>
                    <li><strong>Dinlenme:</strong> Günde en az 7-8 saat uyku.</li>
                </ul>
                <div class='alert alert-success'>🔥 Hedef: Her antrenmanda ağırlıkları artırmaya çalışın!</div>";
            }
            else
            {
                plan = "<h4>🧘 Sağlıklı Yaşam Planı</h4><p>Dengeli beslenme ve düzenli yürüyüş önerilir.</p>";
            }

            // Sonucu modele ekle ve View'a gönder
            model.AIResponse = plan;
            return View("Index", model);
        }
    }
}