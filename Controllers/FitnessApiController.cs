using Microsoft.AspNetCore.Mvc;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApp.Controllers
{
    // Bu API, dış dünyadaki uygulamalara (Mobil App vb.) veri sağlar.
    // Erişim Adresi: /api/fitness
    [Route("api/fitness")]
    [ApiController]
    public class FitnessApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FitnessApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Tüm Eğitmenleri Getir (GET: api/fitness/trainers)
        [HttpGet("trainers")]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
        {
            // Veritabanından tüm eğitmenleri liste olarak çeker
            return await _context.Trainers.ToListAsync();
        }

        // 2. Uzmanlık Alanına Göre Filtrele (LINQ Kullanımı - Şart 4 ve 5)
        // Örnek Kullanım: api/fitness/search?skill=Yoga
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Trainer>>> SearchTrainers(string skill)
        {
            if (string.IsNullOrEmpty(skill))
            {
                return BadRequest("Lütfen bir uzmanlık alanı (skill) belirtin.");
            }

            // LINQ Sorgusu: Uzmanlık alanı girilen kelimeyi içerenleri filtrele
            var trainers = await _context.Trainers
                                         .Where(t => t.Specialization.Contains(skill))
                                         .ToListAsync();

            if (!trainers.Any())
            {
                return NotFound("Bu uzmanlık alanında eğitmen bulunamadı.");
            }

            return trainers;
        }

        // 3. İstatistikler (Raporlama Örneği)
        // GET: api/fitness/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats()
        {
            // Basit bir rapor nesnesi döndürür (JSON formatında)
            var stats = new
            {
                TotalTrainers = await _context.Trainers.CountAsync(),
                TotalServices = await _context.Services.CountAsync(),
                TotalAppointments = await _context.Appointments.CountAsync(),
                LastUpdate = DateTime.Now
            };

            return Ok(stats);
        }
    }
}