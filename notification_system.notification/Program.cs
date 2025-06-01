using notification_system.notification.Extensions;
using notification_system.notification.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Microsoft.Extensions.Logging.ILogger logger = LoggerFactory
    .Create(builder => builder.AddConsole())
    .CreateLogger<Program>();

builder.Services.AddPersistence(builder);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseSerilogRequestLogging(opt =>
{
    opt.EnrichDiagnosticContext = Enricher.HttpRequestEnricher;
});

app.UseCors("CORSPolicy");

app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
