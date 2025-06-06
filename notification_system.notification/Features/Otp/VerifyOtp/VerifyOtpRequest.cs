namespace notification_system.notification.Features.Otp.VerifyOtp;

public class VerifyOtpRequest
{
    public string RefId { get; set; }
    public int OtpValue { get; set; }
    public string Email { get; set; }
}
