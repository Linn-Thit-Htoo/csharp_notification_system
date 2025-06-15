using FluentValidation;

namespace notification_system.notification.Features.PushNoti;

public class BL_PushNoti
{
    private readonly IPushNotiService _pushNotiService;
    private readonly IValidator<PushNotiRequest> _validator;

    public BL_PushNoti(IPushNotiService pushNotiService, IValidator<PushNotiRequest> validator)
    {
        _pushNotiService = pushNotiService;
        _validator = validator;
    }

    public async Task<BaseResponse<PushNotiResponse>> PushNotiAsync(
        PushNotiRequest request,
        CancellationToken cs = default
    )
    {
        BaseResponse<PushNotiResponse> result;

        var validationResult = await _validator.ValidateAsync(request, cs);
        if (!validationResult.IsValid)
        {
            result = BaseResponse<PushNotiResponse>.Fail(
                string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage))
            );
            goto result;
        }

        result = await _pushNotiService.PushNotiAsync(request, cs);

    result:
        return result;
    }
}
