namespace notification_system.notification.Services.SMSServices;

public interface ITwilioService
{
    Task SendSingleSMSAsync(SendSingleSMSRequest request, CancellationToken cs = default);
    Task SendMultipleSMSAsync(SendMultipleSMSRequest request, CancellationToken cs = default);
}
