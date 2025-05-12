using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PV260.Project.Components.ReportsComponent;
using PV260.Project.Components.ReportsComponent.Jobs;
using PV260.Project.Components.ReportsComponent.Services;
using PV260.Project.Components.UsersComponent;
using PV260.Project.Components.UsersComponent.Services;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Interfaces.ArkFunds;
using PV260.Project.Domain.Interfaces.Email;
using PV260.Project.Domain.Interfaces.Persistence;
using PV260.Project.Domain.Options.ArkFundsApi;
using PV260.Project.Domain.Options.SMTP;
using PV260.Project.Infrastructure.ArkFunds.Repositories;
using PV260.Project.Infrastructure.Email;
using PV260.Project.Infrastructure.Persistence;
using PV260.Project.Infrastructure.Persistence.Models;
using PV260.Project.Infrastructure.Persistence.Repositories;
using Quartz;
using Quartz.AspNetCore;

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

        _ = builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        _ = builder.Services.AddScoped<IUserService, UserService>();
        _ = builder.Services.AddScoped<IUserComponent, UserComponent>();
        _ = builder.Services.AddScoped<IEmailSender, EmailSender>();
        _ = builder.Services.AddScoped<IReportService, ReportService>();
        _ = builder.Services.AddScoped<IReportsComponent, ReportsComponent>();

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

    public static WebApplicationBuilder ConfigureQuartzJobs(this WebApplicationBuilder builder)
    {
        _ = builder.Services.AddTransient<IJob, GenerateReportJob>();

        _ = builder.Services.AddQuartz(q =>
        {
            var generateReportJobKey = new JobKey("GenerateReportJob");

            _ = q.AddJob<GenerateReportJob>(opt => opt.WithIdentity(generateReportJobKey));

            _ = q.AddTrigger(opts => opts
                    .ForJob(generateReportJobKey)
                    .WithIdentity("GenerateReportJob-trigger")
                    .WithCronSchedule("0 0 2 ? * * *") // At 02:00:00am every day
                );
        });

        _ = builder.Services.AddQuartzServer(opt => opt.WaitForJobsToComplete = true);

        return builder;
    }
}