using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PV260.Project.DataAccessLayer.Data;
using PV260.Project.DataAccessLayer.Models;
using PV260.Project.Server.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string envName = builder.Environment.EnvironmentName;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
    .Build();

string sqliteConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

// Add services to the container.
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    _ = options
       .UseSqlite(sqliteConnectionString)
       .LogTo(a => Console.WriteLine(a), LogLevel.Debug)
       .EnableSensitiveDataLogging(true)
       .UseLazyLoadingProxies();
    ;
});

// Add Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Set cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    // For API endpoints, return 401 status code instead of redirecting to login page
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie("Identity.Bearer");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGroup("/api/User")
   .MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
