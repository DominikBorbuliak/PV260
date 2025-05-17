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
        .ConfigureQuartzJobs()
        .ConfigureLogger()
        .Build();

    app
        .ApplyMigrations()
        .SeedRoles()
        .SeedAdminUser()
        .SeedUser()
        .ConfigureApplication()
        .Run();
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
}