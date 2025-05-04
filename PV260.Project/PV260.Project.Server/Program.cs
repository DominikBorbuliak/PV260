using PV260.Project.Server.Data;
using PV260.Project.Server.Extensions;

try
{
    WebApplication app = WebApplication.CreateBuilder(args)
        .ConfigureDatabase()
        .ConfigureOptions()
        .ConfigureServices()
        .ConfigureAuth()
        .ConfigureArkHttpClient()
        .ConfigureControllers()
        .Build();
    
    app.ApplyMigrations();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            await SeedData.EnsureRolesAsync(services);
            await SeedData.EnsureAdminUserAsync(services);
            await SeedData.EnsureSeedUsersAsync(services);
        }
        catch (Exception ex)
        {
            ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error while seeding database");
        }
    }

    app.ConfigureApplication().Run();
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
}