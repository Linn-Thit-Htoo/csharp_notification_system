using Consul;
using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Entities;
using notification_system.notification.Exceptions;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Features.Otp.RequestOtp;
using notification_system.notification.Features.Otp.VerifyOtp;
using notification_system.notification.Features.PushNoti;
using notification_system.notification.Features.SMS.SendSMS;
using notification_system.notification.Persistence.Wrapper;
using notification_system.notification.Services.EmailServices;
using notification_system.notification.Services.PushNoti;
using notification_system.notification.Services.RabbitMQ;
using notification_system.notification.Services.ServiceDiscovery;
using notification_system.notification.Services.SMSServices;
using notification_system.notification.Utils;
using Serilog;

namespace notification_system.notification.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder
            .Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();

        builder
            .Services.AddControllers()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context
                        .ModelState.Where(ms => ms.Value!.Errors.Any())
                        .SelectMany(ms => ms.Value!.Errors.Select(e => e.ErrorMessage))
                        .ToList();

                    var errorMessage = string.Join(" ", errors);
                    var result = BaseResponse<object>.Fail(errorMessage);

                    return new OkObjectResult(result);
                };
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

        builder.Services.AddDbContext<NotificationDbContext>(
            (sp, opt) =>
            {
                var setting = sp.GetRequiredService<IOptions<AppSetting>>().Value;
                opt.UseSqlServer(setting.ConnectionStrings.NotiConnection);
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        );

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "CORSPolicy",
                builder =>
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed((hosts) => true)
            );
        });

        FirebaseApp.Create(
            new AppOptions()
            {
                Credential = GoogleCredential.FromFile(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "notification-system-b95f7-firebase-adminsdk-fbsvc-e613046e55.json"
                    )
                ),
            }
        );

        builder.Host.UseSerilog(
            (context, config) =>
            {
                config
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentName();
            }
        );

        if (!builder.Environment.IsDevelopment())
        {
            builder.Services.AddConsul(builder);
            builder.Services.AddHostedService<ConsulService>();

            builder.Host.UseSerilog(
                (context, config) =>
                {
                    config
                        .ReadFrom.Configuration(context.Configuration)
                        .WriteTo.Console()
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithEnvironmentName()
                        .WriteTo.Seq("http://seq:5341");
                }
            );
        }

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.Configure<AppSetting>(builder.Configuration);
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
        builder.Services.AddBusinessLogicServices();
        builder.Services.AddDataAccessServices();
        builder.Services.AddHostedService<RabbitMQService>();
        builder.Services.AddScoped<IPushNotiService, PushNotiService>();
        builder.Services.AddScoped<ITwilioService, TwilioService>();

        return services;
    }

    private static IServiceCollection AddConsul(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        var consulClient = new ConsulClient(config =>
        {
            config.Address = new Uri(builder.Configuration["Consul:DiscoveryAddress"]!);
        });

        services.AddSingleton<IConsulClient, ConsulClient>(_ => consulClient);
        services.AddHostedService<ConsulService>();

        return services;
    }

    private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<BL_SendEmail>();
        services.AddScoped<BL_RequestOtp>();
        services.AddScoped<BL_VerifyOtp>();
        services.AddScoped<BL_PushNoti>();
        services.AddScoped<BL_SendSMS>();
        return services;
    }

    private static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddScoped<DA_RequestOtp>();
        services.AddScoped<DA_VerifyOtp>();
        return services;
    }
}
