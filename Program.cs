using FitnessCenterApp.Data;
using FitnessCenterApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<StabilityAIService>();
builder.Services.AddScoped<GeminiAIService>();
// Servislerin konteynere eklenmesi
builder.Services.AddControllersWithViews();

// Veritabanı Bağlantısı (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------------------------------
// KOD 1: Kimlik (Identity) Ayarları
// ---------------------------------------------------------
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Şifre kurallarını basitleştiriyoruz (Proje şartı: "sau" şifresi için)
    options.Password.RequireDigit = false;            // Rakam zorunlu değil
    options.Password.RequireLowercase = false;        // Küçük harf zorunlu değil
    options.Password.RequireUppercase = false;        // Büyük harf zorunlu değil
    options.Password.RequireNonAlphanumeric = false;  // Sembol (!@#) zorunlu değil
    options.Password.RequiredLength = 3;              // En az 3 karakter (s-a-u)
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// HTTP istek boru hattının yapılandırılması (Configure the HTTP request pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ---------------------------------------------------------
// KOD 2: Kimlik Doğrulama (Authentication)
// Önemli: Bu kod, UseAuthorization satırından önce gelmelidir.
// ---------------------------------------------------------
app.UseAuthentication(); // Kullanıcının kim olduğunu doğrular

app.UseAuthorization();  // Kullanıcının yetkisini kontrol eder

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ---------------------------------------------------------
// KOD 3: Razor Sayfalarının Haritalanması
// Login ve Register sayfalarının çalışması için gereklidir.
// ---------------------------------------------------------
app.MapRazorPages();
// Veritabanına başlangıç verilerini (Admin) ekleme işlemi
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await FitnessCenterApp.Data.DbSeeder.SeedRolesAndAdminAsync(services);
}

app.Run();
app.Run();