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
       
        [Display(Name = "Mesai Başlangıç Saati (0-23)")]
        [Range(0, 23)]
        public int WorkStartHour { get; set; } = 9; // Varsayılan 09:00

        [Display(Name = "Mesai Bitiş Saati (0-23)")]
        [Range(0, 23)]
        public int WorkEndHour { get; set; } = 18; // Varsayılan 18:00

        // Antrenörün verdiği randevular (İlişki)
        public ICollection<Appointment>? Appointments { get; set; }
    }
}