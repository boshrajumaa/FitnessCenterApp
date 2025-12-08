using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    // Üyelerin aldığı randevuları temsil eden sınıf
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Randevu Tarihi")]
        [Required]
        public DateTime AppointmentDate { get; set; } // Randevunun tarihi ve saati

        [Display(Name = "Durum")]
        public bool IsConfirmed { get; set; } // Randevu onaylandı mı?

        // İlişkiler (Foreign Keys)

        // Hangi Üye aldı? (Şimdilik string olarak tutuyoruz, Identity eklenince bağlanacak)
        public string? MemberId { get; set; }

        // Hangi Hizmet?
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        // Hangi Antrenör?
        [Display(Name = "Antrenör")]
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}