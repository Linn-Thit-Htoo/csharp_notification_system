namespace notification_system.notification.Features.SMS.SendSMS;

public class SendSingleSMSRequest
{
    public string ToPhoneNumber { get; set; }
    public string Messasge { get; set; }
}
