namespace notification_system.notification.Features.Email.SendEmail;

public class SendSingleEmailValidator : AbstractValidator<SendEmailRequest>
{
    public SendSingleEmailValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject cannot be empty.")
            .NotNull()
            .WithMessage("Subject cannto be null.");

        RuleFor(x => x.HtmlContent)
            .NotEmpty()
            .WithMessage("Html Content cannot be empty.")
            .NotNull()
            .WithMessage("Html Content cannot be null.");

        RuleFor(x => x.ToEmail)
            .NotEmpty()
            .WithMessage("To Email cannot be empty.")
            .NotNull()
            .WithMessage("To Email cannot be null.")
            .EmailAddress()
            .WithMessage("Email is invailid.");
    }
}
