namespace notification_system.notification.Services.PushNoti;

public interface IPushNotiService
{
    Task<BaseResponse<PushNotiResponse>> PushNotiAsync(
        PushNotiRequest request,
        CancellationToken cs = default
    );
}
