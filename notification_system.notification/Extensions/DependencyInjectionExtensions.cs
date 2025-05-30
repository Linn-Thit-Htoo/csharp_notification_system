using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Entities;
using notification_system.notification.Exceptions;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Features.Otp.RequestOtp;
using notification_system.notification.Persistence.Wrapper;
using notification_system.notification.Services.EmailServices;
using notification_system.notification.Utils;

namespace notification_system.notification.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, WebApplicationBuilder builder)
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

            builder.Services.AddDbContext<NotificationDbContext>((sp, opt) =>
            {
                var setting = sp.GetRequiredService<IOptions<AppSetting>>().Value;
                opt.UseSqlServer(setting.ConnectionStrings.NotiConnection);
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

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

            return services;
        }

        private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<BL_SendEmail>();
            services.AddScoped<BL_RequestOtp>();
            return services;
        }

        private static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<DA_RequestOtp>();
            return services;
        }
    }
}
