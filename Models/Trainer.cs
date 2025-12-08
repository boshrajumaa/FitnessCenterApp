using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    // Spor salonunda çalışan antrenörleri temsil eden sınıf
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } // Antrenörün tam adı

        [Required]
        [Display(Name = "Uzmanlık Alanı")]
        public string Specialization { get; set; } // Örn: Kilo Verme, Kas Kazanma

        [Display(Name = "Fotoğraf")]
        public string? ImageUrl { get; set; } // Antrenörün fotoğrafı için dosya yolu

        // Antrenörün verdiği randevular (İlişki)
        public ICollection<Appointment>? Appointments { get; set; }
    }
}