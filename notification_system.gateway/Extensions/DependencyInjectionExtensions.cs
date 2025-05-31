using Consul;
using Microsoft.Extensions.Options;
using notification_system.gateway.Configurations;
using notification_system.gateway.Services.ServiceDiscovery;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;

namespace notification_system.gateway.Extensions
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
                .AddJsonFile(
                $"ocelot.{builder.Environment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true
                )
                .AddEnvironmentVariables();

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.AddConsul(builder);
                builder.Services.AddHostedService<ConsulService>();
            }

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder
                .Services.AddOcelot()
                .AddConsul();
            builder.Services.AddHealthChecks();
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<AppSetting>(builder.Configuration);

            return services;
        }

        private static IServiceCollection AddConsul(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(builder.Configuration["Consul:DiscoveryAddress"]!);
            });

            services.AddSingleton<IConsulClient, ConsulClient>(_ => consulClient);
            services.AddHostedService<ConsulService>();

            return services;
        }
    }
}
