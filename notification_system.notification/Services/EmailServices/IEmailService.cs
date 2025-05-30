using notification_system.notification.Features.Email.SendEmail;

namespace notification_system.notification.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailRequest request, CancellationToken cs = default);
        Task SendEmailAysnc(SendEmailMultipleRequest request, CancellationToken cs = default);
    }
}
