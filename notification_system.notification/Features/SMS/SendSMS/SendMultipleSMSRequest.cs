namespace notification_system.notification.Features.SMS.SendSMS;

public class SendMultipleSMSRequest
{
    public List<string> ToPhoneNumbers { get; set; }
    public string Messasge { get; set; }
}
