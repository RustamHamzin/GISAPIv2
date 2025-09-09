using GisZhkhAdmin.Models;
using Microsoft.AspNetCore.Identity;

namespace GisZhkhAdmin.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            string[] roleNames = { "Administrator", "Observer", "AreaProgrammer" };
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user
            var adminUser = await userManager.FindByEmailAsync("admin@giszhkh.local");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@giszhkh.local",
                    Email = "admin@giszhkh.local",
                    FirstName = "Администратор",
                    LastName = "Системы",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            // Create observer user
            var observerUser = await userManager.FindByEmailAsync("observer@giszhkh.local");
            if (observerUser == null)
            {
                observerUser = new ApplicationUser
                {
                    UserName = "observer@giszhkh.local",
                    Email = "observer@giszhkh.local",
                    FirstName = "Наблюдатель",
                    LastName = "Системы",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(observerUser, "Observer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(observerUser, "Observer");
                }
            }

            // Seed sample contracts
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            if (!context.Contracts.Any())
            {
                var contracts = new List<Contract>
                {
                    new Contract { Number = "001/2024", SignDate = DateTime.Now.AddDays(-30), StartDate = DateTime.Now.AddDays(-30), StatusId = 2, Description = "Договор на обслуживание дома №1" },
                    new Contract { Number = "002/2024", SignDate = DateTime.Now.AddDays(-25), StartDate = DateTime.Now.AddDays(-25), StatusId = 1, Description = "Договор на обслуживание дома №2" },
                    new Contract { Number = "003/2024", SignDate = DateTime.Now.AddDays(-20), StartDate = DateTime.Now.AddDays(-20), StatusId = 3, Description = "Договор на обслуживание дома №3" },
                    new Contract { Number = "004/2024", SignDate = DateTime.Now.AddDays(-15), StartDate = DateTime.Now.AddDays(-15), StatusId = 4, Description = "Договор на обслуживание дома №4" },
                    new Contract { Number = "005/2024", SignDate = DateTime.Now.AddDays(-10), StartDate = DateTime.Now.AddDays(-10), StatusId = 2, Description = "Договор на обслуживание дома №5" }
                };

                context.Contracts.AddRange(contracts);
                await context.SaveChangesAsync();
            }
        }
    }
}