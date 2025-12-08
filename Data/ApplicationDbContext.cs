using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Data
{
    // Veritabanı bağlam sınıfı (Veritabanı ile iletişim kuran köprü)
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanı tablolarımızı temsil eden DbSet tanımları
        // Bu kodlar, C# sınıflarını SQL tablolarına dönüştürür
        public DbSet<Trainer> Trainers { get; set; }     // Eğitmenler Tablosu
        public DbSet<Service> Services { get; set; }     // Hizmetler Tablosu
        public DbSet<Appointment> Appointments { get; set; } // Randevular Tablosu
    }
}