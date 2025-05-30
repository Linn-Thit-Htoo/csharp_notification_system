namespace notification_system.notification.Features.Otp.RequestOtp
{
    public class RequestOtpResponse
    {
        public int Otp { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
