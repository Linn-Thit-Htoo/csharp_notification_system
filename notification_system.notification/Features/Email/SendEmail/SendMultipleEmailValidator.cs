namespace notification_system.notification.Features.Email.SendEmail;

public class SendMultipleEmailValidator : AbstractValidator<SendMultipleEmailRequest>
{
    public SendMultipleEmailValidator()
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

        RuleFor(x => x.ToEmails).NotNull().WithMessage("To Emails cannot be null.");
    }
}
