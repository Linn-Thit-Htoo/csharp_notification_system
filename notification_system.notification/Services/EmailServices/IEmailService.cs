namespace notification_system.notification.Services.EmailServices;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailRequest request, CancellationToken cs = default);
    Task SendMultipleEmailAysnc(SendMultipleEmailRequest request, CancellationToken cs = default);
}
