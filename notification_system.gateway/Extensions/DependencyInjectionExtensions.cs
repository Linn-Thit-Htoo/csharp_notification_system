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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
