using notification_system.notification.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("CORSPolicy");

app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
