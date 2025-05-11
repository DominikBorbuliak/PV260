using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PV260.Project.Infrastructure.Persistence;
using PV260.Project.Infrastructure.Persistence.Models;
using PV260.Project.Server.Middlewares;

namespace PV260.Project.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        _ = app.UseDefaultFiles();
        _ = app.UseStaticFiles();

        _ = app.MapGroup("/api/User")
            .MapIdentityApi<UserEntity>();

        if (app.Environment.IsDevelopment())
        {
            _ = app.MapOpenApi();
        }

        _ = app.UseMiddleware<ExceptionMiddleware>();

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        _ = app.MapFallbackToFile("/index.html");

        return app;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IDbContextFactory<AppDbContext> factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        using AppDbContext db = factory.CreateDbContext();
        db.Database.Migrate();
        return app;
    }
    
    public static WebApplication SeedRoles(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        RoleManager<RoleEntity> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();

        foreach (string roleName in new[] { "Admin", "User" })
        {
            if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new RoleEntity { Name = roleName })
                    .GetAwaiter().GetResult();
            }
        }

        return app;
    }
    
    public static WebApplication SeedAdminUser(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        UserManager<UserEntity> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        const string adminEmail    = "admin@admin.com";
        const string adminPassword = "Adm!n123";

        if (userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult() is not null)
        {
            return app;
        }

        var admin = new UserEntity
        {
            UserName       = adminEmail,
            Email          = adminEmail,
            EmailConfirmed = true
        };

        IdentityResult result = userManager.CreateAsync(admin, adminPassword)
            .GetAwaiter().GetResult();

        if (!result.Succeeded)
        {
            throw new Exception(
                $"Seeding admin user failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
        }

        userManager.AddToRoleAsync(admin, "Admin")
            .GetAwaiter().GetResult();

        return app;
    }
    
    public static WebApplication SeedUser(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        UserManager<UserEntity> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        const string userEmail    = "user@user.com";
        const string userPassword = "User123!";

        if (userManager.FindByEmailAsync(userEmail).GetAwaiter().GetResult() is not null)
        {
            return app;
        }

        var user = new UserEntity
        {
            UserName       = userEmail,
            Email          = userEmail,
            EmailConfirmed = true
        };

        IdentityResult result = userManager.CreateAsync(user, userPassword)
            .GetAwaiter().GetResult();

        if (!result.Succeeded)
        {
            throw new Exception(
                $"Seeding user failed: {string.Join("; ", result.Errors.Select(e => e.Description))}");
        }

        userManager.AddToRoleAsync(user, "User")
            .GetAwaiter().GetResult();

        return app;
    }
}