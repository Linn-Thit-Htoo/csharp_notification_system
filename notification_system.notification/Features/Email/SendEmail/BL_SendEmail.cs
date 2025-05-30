using FluentValidation;
using notification_system.notification.Services.EmailServices;
using notification_system.notification.Utils;

namespace notification_system.notification.Features.Email.SendEmail
{
    public class BL_SendEmail
    {
        private readonly IEmailService _emailService;
        private readonly IValidator<SendEmailRequest> _sendSingleEmailValidator;

        public BL_SendEmail(IEmailService emailService, IValidator<SendEmailRequest> sendSingleEmailValidator)
        {
            _emailService = emailService;
            _sendSingleEmailValidator = sendSingleEmailValidator;
        }

        public async Task<BaseResponse<SendEmailResponse>> SendSingleEmailAsync(SendEmailRequest request, CancellationToken cs = default)
        {
            BaseResponse<SendEmailResponse> result;
            try
            {
                var validationResult = await _sendSingleEmailValidator.ValidateAsync(request, cs);
                if (!validationResult.IsValid)
                {
                    result = BaseResponse<SendEmailResponse>.Fail(string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage)));
                    goto result;
                }

                await _emailService.SendEmailAsync(request, cs);
                result = BaseResponse<SendEmailResponse>.Success();
            }
            catch (Exception ex)
            {
                result = BaseResponse<SendEmailResponse>.Fail(ex);
            }

        result:
            return result;
        }
    }
}
