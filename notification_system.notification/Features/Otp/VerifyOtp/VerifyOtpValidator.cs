using FluentValidation;

namespace notification_system.notification.Features.Otp.VerifyOtp;

public class VerifyOtpValidator : AbstractValidator<VerifyOtpRequest>
{
    public VerifyOtpValidator()
    {
        RuleFor(x => x.RefId)
            .NotEmpty()
            .WithMessage("Ref Id cannot be empty.")
            .NotNull()
            .WithMessage("Ref Id cannot be null.");

        RuleFor(x => x.OtpValue).GreaterThan(0).WithMessage("Invalid Otp.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .NotNull()
            .WithMessage("Email cannot be null.")
            .EmailAddress()
            .WithMessage("Email is invailid.");
    }
}
