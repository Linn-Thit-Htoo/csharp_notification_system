namespace notification_system.notification.Features.Otp.RequestOtp;

public class RequestOtpValidator : AbstractValidator<RequestOtpRequest>
{
    public RequestOtpValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .NotNull()
            .WithMessage("Email cannot be null.")
            .EmailAddress()
            .WithMessage("Email is invailid.");
    }
}
