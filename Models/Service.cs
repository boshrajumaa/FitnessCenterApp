using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    // Spor salonunda sunulan hizmetleri temsil eden sınıf (Örn: Pilates, Yoga, Fitness)
    public class Service
    {
        [Key]
        public int Id { get; set; } // Birincil Anahtar (Primary Key)

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } // Hizmetin adı (Yoga, Pilates vb.)

        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; } // Hizmetin süresi (dk cinsinden)

        [Display(Name = "Ücret")]
        public decimal Price { get; set; } // Hizmetin ücreti

        [Display(Name = "Açıklama")]
        public string? Description { get; set; } // Hizmet hakkında kısa bilgi

        // Bu hizmete ait randevuların listesi (İlişki)
        public ICollection<Appointment>? Appointments { get; set; }
    }
}