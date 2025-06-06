using FluentValidation;
using notification_system.notification.Utils;

namespace notification_system.notification.Features.Otp.RequestOtp;

public class BL_RequestOtp
{
    private readonly IValidator<RequestOtpRequest> _validator;
    private readonly DA_RequestOtp _dA_RequestOtp;

    public BL_RequestOtp(IValidator<RequestOtpRequest> validator, DA_RequestOtp dA_RequestOtp)
    {
        _validator = validator;
        _dA_RequestOtp = dA_RequestOtp;
    }

    public async Task<BaseResponse<RequestOtpResponse>> RequestOtp(
        RequestOtpRequest request,
        CancellationToken cs = default
    )
    {
        BaseResponse<RequestOtpResponse> result;

        var validationResult = await _validator.ValidateAsync(request, cs);
        if (!validationResult.IsValid)
        {
            result = BaseResponse<RequestOtpResponse>.Fail(
                string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage))
            );
            goto result;
        }

        result = await _dA_RequestOtp.RequestOtp(request, cs);

    result:
        return result;
    }
}
