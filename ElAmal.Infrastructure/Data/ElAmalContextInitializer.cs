using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElAmal.Infrastructure.Data
{
    public class ElAmalContextInitializer (ElAmalDbContext _dbContext) : IElAmalContextInitializer
    {   
        public async Task UpdateDatabaseAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();
        }

        public async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var userRole =  serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await userRole.RoleExistsAsync("Admin"))
                await userRole.CreateAsync(new IdentityRole("Admin"));

            //Create Admin User
            var adminEmail = configuration["AdminUser:Mail"];
            var adminPassword = configuration["AdminUser:Password"];
            var companyName = configuration["AdminUser:CompanyName"];
            var recoveryEmail = configuration["AdminUser:RecoveryEmail"];

            //Validation 
            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword)
                || string.IsNullOrWhiteSpace(recoveryEmail))
            {
                Console.WriteLine("[SEED] Email or Password is missing in configuration");
                return;
            }

            //Check on Admin 
            var existingAdnin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdnin is null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail.Split('@')[0],
                    Email = adminEmail,
                    EmailConfirmed = true,
                    CompanyEmail = companyName!,
                    RecoveryEmail = recoveryEmail
                };

                var result = await userManager.CreateAsync(admin, adminPassword!);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine("[SEED] Admin user created successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"[SEED ERROR] {error.Code}: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("[SEED] Admin user already exists.");
            }

        }

    }
}
