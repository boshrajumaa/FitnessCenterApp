using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    // Kullanıcıdan AI (Yapay Zeka) için alınacak veriler
    public class AIPlanViewModel
    {
        [Display(Name = "Boyunuz (cm)")]
        [Required(ErrorMessage = "Lütfen boyunuzu giriniz.")]
        [Range(100, 250, ErrorMessage = "Geçerli bir boy giriniz.")]
        public int Height { get; set; }

        [Display(Name = "Kilonuz (kg)")]
        [Required(ErrorMessage = "Lütfen kilonuzu giriniz.")]
        [Range(30, 200, ErrorMessage = "Geçerli bir kilo giriniz.")]
        public int Weight { get; set; }

        [Display(Name = "Hedefiniz")]
        [Required]
        public string Goal { get; set; } // Kilo Verme, Kas Kazanma, vb.

        // AI'nın ürettiği cevap buraya gelecek
        public string? AIResponse { get; set; }
    }
}