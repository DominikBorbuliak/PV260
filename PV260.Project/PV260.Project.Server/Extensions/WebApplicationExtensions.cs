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
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        using var db = factory.CreateDbContext();
        db.Database.Migrate();
        return app;
    }
}