using FluentValidation;

namespace notification_system.notification.Features.PushNoti
{
    public class PushNotiValidator : AbstractValidator<PushNotiRequest>
    {
        public PushNotiValidator()
        {
            RuleFor(x => x.DeviceToken)
                .NotEmpty().WithMessage("Device Token cannot be empty.")
                .NotNull().WithMessage("Device Token cannot be null.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .NotNull().WithMessage("Title cannot be null.");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body cannot be empty.")
                .NotNull().WithMessage("Body cannot be null.");
        }
    }
}
