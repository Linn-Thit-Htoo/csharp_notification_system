using notification_system.notification.Extensions;

namespace notification_system.notification.Features.SMS.SendSMS;

public class BL_SendSMS
{
    private readonly ITwilioService _twilioService;

    public BL_SendSMS(ITwilioService twilioService)
    {
        _twilioService = twilioService;
    }

    public async Task<BaseResponse<SendSMSResponse>> SendSingleSMSAsync(
        SendSingleSMSRequest request,
        CancellationToken cs = default
    )
    {
        BaseResponse<SendSMSResponse> result;

        if (request.ToPhoneNumber.IsNullOrEmpty())
        {
            result = BaseResponse<SendSMSResponse>.Fail("To Phone Number cannot be empty.");
            goto result;
        }

        if (request.Messasge.IsNullOrEmpty())
        {
            result = BaseResponse<SendSMSResponse>.Fail("Message cannot be empty.");
            goto result;
        }

        await _twilioService.SendSingleSMSAsync(request, cs);
        result = BaseResponse<SendSMSResponse>.Success();

    result:
        return result;
    }

    public async Task<BaseResponse<SendSMSResponse>> SendMultipleSMSAsync(
        SendMultipleSMSRequest request,
        CancellationToken cs = default
    )
    {
        BaseResponse<SendSMSResponse> result;

        if (request.ToPhoneNumbers is null || request.ToPhoneNumbers.Count <= 0)
        {
            result = BaseResponse<SendSMSResponse>.Fail("To Phone Number cannot be empty.");
            goto result;
        }

        if (request.Messasge.IsNullOrEmpty())
        {
            result = BaseResponse<SendSMSResponse>.Fail("Message cannot be empty.");
            goto result;
        }

        await _twilioService.SendMultipleSMSAsync(request, cs);
        result = BaseResponse<SendSMSResponse>.Success();

    result:
        return result;
    }
}
