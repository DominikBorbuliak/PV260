using PV260.Project.Server.Extensions;

try
{
    WebApplication.CreateBuilder(args)
        .ConfigureDatabase()
        .ConfigureOptions()
        .ConfigureServices()
        .ConfigureAuth()
        .ConfigureArkHttpClient()
        .ConfigureControllers()
        .Build()
        .ConfigureApplication()
        .Run();
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
}
