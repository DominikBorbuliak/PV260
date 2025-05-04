using Microsoft.AspNetCore.Identity;

using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Server.Data
{
    public static class SeedData
    {
        public static async Task EnsureRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<RoleEntity>>();
            string[] roles = { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new RoleEntity { Name = roleName });
                }
            }
        }
        
        public static async Task EnsureAdminUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<UserEntity>>();
            const string adminEmail = "admin@admin.com";
            const string adminPassword = "Adm!n123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new UserEntity
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Seeding admin user failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
                }

                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
        
        public static async Task EnsureSeedUsersAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<UserEntity>>();
            const string userEmail = "user@user.com";
            const string userPassword = "User123!";

            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new UserEntity
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, userPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Seeding admin user failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
                }

                await userManager.AddToRoleAsync(user, "User");
            }
        }

    }
}
