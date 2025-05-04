using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Interfaces.Domain;
using PV260.Project.Domain.Interfaces.Infrastructure.ArkFunds;
using PV260.Project.Domain.Interfaces.Infrastructure.Email;
using PV260.Project.Domain.Interfaces.Infrastructure.Persistence;
using PV260.Project.Domain.Options.ArkFundsApi;
using PV260.Project.Domain.Options.SMTP;
using PV260.Project.Domain.Services;
using PV260.Project.Infrastructure.ArkFunds.Repositories;
using PV260.Project.Infrastructure.Email;
using PV260.Project.Infrastructure.Persistence;
using PV260.Project.Infrastructure.Persistence.Models;
using PV260.Project.Infrastructure.Persistence.Repositories;

namespace PV260.Project.Server.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        string sqliteConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                        ?? throw new ConfigurationException("Invalid application configuration.");

        _ = builder.Services.AddDbContextFactory<AppDbContext>(options =>
        {
            _ = options
                .UseSqlite(sqliteConnectionString)
                .LogTo(a => Console.WriteLine(a), LogLevel.Debug)
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors()
                .UseLazyLoadingProxies();
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
    {
        _ = builder.Services.Configure<ArkFundsApiOptions>(builder.Configuration.GetSection(ArkFundsApiOptions.Key));
        _ = builder.Services.Configure<SMTPOptions>(builder.Configuration.GetSection(SMTPOptions.Key));

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
    {
        _ = builder.Services.AddIdentity<UserEntity, RoleEntity>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();

        _ = builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        _ = builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

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

        _ = builder.Services.AddAuthorization();
        _ = builder.Services.AddAuthentication().AddCookie("Identity.Bearer");

        return builder;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        _ = builder.Services.AddScoped<IArkFundsApiRepository, ArkFundsApiRepository>();
        _ = builder.Services.AddScoped<IUserRepository, UserRepository>();
        _ = builder.Services.AddScoped<IReportRepository, ReportRepository>();

        _ = builder.Services.AddScoped<IUserService, UserService>();
        _ = builder.Services.AddScoped<IEmailSender, EmailSender>();
        _ = builder.Services.AddScoped<IReportService, ReportService>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureArkHttpClient(this WebApplicationBuilder builder)
    {
        ArkFundsApiOptions arksFundsApiOptions = builder.Configuration
                                                     .GetSection(ArkFundsApiOptions.Key)
                                                     .Get<ArkFundsApiOptions>()
                                                 ?? throw new ConfigurationException(
                                                     "Invalid application configuration.");

        _ = builder.Services.AddHttpClient(arksFundsApiOptions.HttpClientKey, client =>
        {
            string baseUrlWithVersion = arksFundsApiOptions.BaseUrl;
            client.BaseAddress = new Uri(baseUrlWithVersion);
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureControllers(this WebApplicationBuilder builder)
    {
        _ = builder.Services.AddControllers();
        _ = builder.Services.AddOpenApi();

        return builder;
    }
}