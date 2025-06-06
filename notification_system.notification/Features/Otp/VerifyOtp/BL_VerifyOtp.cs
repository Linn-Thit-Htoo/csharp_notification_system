using FluentValidation;
using notification_system.notification.Utils;

namespace notification_system.notification.Features.Otp.VerifyOtp;

public class BL_VerifyOtp
{
    private readonly DA_VerifyOtp _dA_VerifyOtp;
    private readonly IValidator<VerifyOtpRequest> _validator;

    public BL_VerifyOtp(DA_VerifyOtp dA_VerifyOtp, IValidator<VerifyOtpRequest> validator)
    {
        _dA_VerifyOtp = dA_VerifyOtp;
        _validator = validator;
    }

    public async Task<BaseResponse<VerifyOtpResponse>> VerifyOtpAsync(
        VerifyOtpRequest request,
        CancellationToken cs = default
    )
    {
        BaseResponse<VerifyOtpResponse> result;

        var validationResult = await _validator.ValidateAsync(request, cs);
        if (!validationResult.IsValid)
        {
            result = BaseResponse<VerifyOtpResponse>.Fail(
                string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage))
            );
            goto result;
        }

        result = await _dA_VerifyOtp.VerifyOtpAsync(request, cs);

        result:
        return result;
    }
}
