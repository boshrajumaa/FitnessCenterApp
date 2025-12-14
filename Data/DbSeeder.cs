using Microsoft.AspNetCore.Identity;

namespace FitnessCenterApp.Data
{
    public static class DbSeeder
    {
        // Bu metod, veritabanına varsayılan Admin ve Rolleri ekler
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Kullanıcı ve Rol yöneticilerini alıyoruz
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Rollerin oluşturulması (Admin ve Üye)
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // 2. Admin kullanıcısının oluşturulması
            // DİKKAT: Aşağıdaki e-postayı kendi öğrenci numaranızla güncelleyin!
            var adminEmail = "B221210552@sakarya.edu.tr"; // Örnek numara

            var userIfExists = await userManager.FindByEmailAsync(adminEmail);
            if (userIfExists == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Kullanıcıyı "sau" şifresiyle oluştur
                await userManager.CreateAsync(adminUser, "sau");

                // Kullanıcıya Admin rolünü ata
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}