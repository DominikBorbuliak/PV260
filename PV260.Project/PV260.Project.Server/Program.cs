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

    _ = app.ApplyMigrations()
        .SeedRoles()
        .SeedAdminUser()
        .SeedUser();

    app.ConfigureApplication().Run();
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
}