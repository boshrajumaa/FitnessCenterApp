using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class AIPlanViewModel
    {
        [Display(Name = "Yaşınız")]
        [Range(10, 100, ErrorMessage = "Geçerli bir yaş giriniz.")]
        public int? Age { get; set; }

        [Display(Name = "Boyunuz (cm)")]
        [Range(100, 250)]
        public int? Height { get; set; }

        [Display(Name = "Kilonuz (kg)")]
        [Range(30, 200)]
        public int? Weight { get; set; }

        [Display(Name = "Cinsiyet")]
        public string? Gender { get; set; }

        [Display(Name = "Vücut Tipi")]
        public string? BodyType { get; set; }

        [Display(Name = "Hedefiniz")]
        public string? Goal { get; set; }

        [Display(Name = "Eklemek İstediğiniz Mesaj (Opsiyonel)")]
        public string? UserMessage { get; set; }


        [Display(Name = "Vücut Fotoğrafınız (Opsiyonel)")]
        public IFormFile? UserImage { get; set; }

        public string? OriginalImageBase64 { get; set; }

        public string? GeneratedImageBase64 { get; set; }
        public string? AIResponse { get; set; }
    }
}