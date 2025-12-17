using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenterApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Appointments.Include(a => a.Service).Include(a => a.Trainer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            // Hizmetleri ve Eğitmenleri Dropdown (Seçim kutusu) için hazırlıyoruz
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Bu metod, üye tarafından yeni bir randevu oluşturulmasını sağlar.
        /// </summary>
        public async Task<IActionResult> Create([Bind("Id,AppointmentDate,MemberId,ServiceId,TrainerId")] Appointment appointment)
        {
            // Kullanıcı giriş yapmış mı kontrolü (Güvenlik)
            if (User.Identity.IsAuthenticated)
            {
                // Randevuyu oluşturan üyenin bilgisini otomatik alıyoruz
                appointment.MemberId = User.Identity.Name;
            }

            // --- KURAL 1: EĞİTMENİN ÇALIŞMA SAATLERİ KONTROLÜ ---
            // 1. Seçilen eğitmenin çalışma saatlerini öğrenmek için veritabanından çekiyoruz
            var selectedTrainer = await _context.Trainers.FindAsync(appointment.TrainerId);

            // 2. Saat Kontrolü
            if (selectedTrainer != null)
            {
                // Eğer seçilen saat, eğitmenin başlama saatinden küçükse veya bitiş saatinden büyükse hata ver
                if (appointment.AppointmentDate.Hour < selectedTrainer.WorkStartHour ||
                    appointment.AppointmentDate.Hour >= selectedTrainer.WorkEndHour)
                {
                    // Bu hata mesajı, View tarafında (Create.cshtml) eklediğimiz 'span' alanında görünecektir
                    ModelState.AddModelError("TrainerId",
                        $"Seçilen eğitmen sadece {selectedTrainer.WorkStartHour}:00 - {selectedTrainer.WorkEndHour}:00 saatleri arasında çalışmaktadır.");
                }
            }

            // --- KURAL 2: GEÇMİŞ TARİH KONTROLÜ ---
            // Kullanıcı geçmişe dönük randevu alamaz
            if (appointment.AppointmentDate < DateTime.Now)
            {
                ModelState.AddModelError("AppointmentDate", "Geçmiş bir tarihe randevu alamazsınız.");
            }

            // --- KURAL 3: ÇAKIŞMA KONTROLÜ (CONFLICT CHECK) ---
            // Seçilen eğitmenin, o gün ve o saatte başka bir randevusu var mı?
            bool isTrainerBusy = await _context.Appointments.AnyAsync(x =>
                x.TrainerId == appointment.TrainerId &&
                x.AppointmentDate.Date == appointment.AppointmentDate.Date && // Aynı gün
                x.AppointmentDate.Hour == appointment.AppointmentDate.Hour);  // Aynı saat

            if (isTrainerBusy)
            {
                ModelState.AddModelError("TrainerId", "Seçilen eğitmen bu saatte doludur. Lütfen başka bir saat seçiniz.");
            }

            // --- 4. VERİTABANINA KAYIT ---
            if (ModelState.IsValid)
            {
                appointment.IsConfirmed = false; // Randevu onaysız olarak başlar
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa listeleri tekrar doldur ve sayfayı göster
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppointmentDate,IsConfirmed,MemberId,ServiceId,TrainerId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", appointment.TrainerId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointments'  is null.");
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
