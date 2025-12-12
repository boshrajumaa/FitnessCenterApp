using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // المكتبة الجديدة
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Data
{
    // IdentityDbContext sınıfından türetiyoruz ki Kullanıcı ve Rol tabloları gelsin
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}