using FirebaseAdmin.Messaging;
using notification_system.notification.Constants;
using notification_system.notification.Entities;
using notification_system.notification.Extensions;
using notification_system.notification.Features.PushNoti;
using notification_system.notification.Persistence.Wrapper;
using notification_system.notification.Utils;

namespace notification_system.notification.Services.PushNoti;

public class PushNotiService : IPushNotiService
{
    private readonly IUnitOfWork _unitOfWork;

    public PushNotiService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<PushNotiResponse>> PushNotiAsync(PushNotiRequest request, CancellationToken cs = default)
    {
        var message = new Message
        {
            Token = request.DeviceToken,
            Notification = new Notification
            {
                Title = request.Title,
                Body = request.Body,
                ImageUrl = request.ImageUrl,
            },
            Data = new Dictionary<string, string>
            {
                { "deepLink", request.DeepLink ?? string.Empty },
            },
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                Notification = new AndroidNotification
                {
                    Title = request.Title,
                    Body = request.Body,
                    ImageUrl = request.ImageUrl
                },
            },
            Apns = new ApnsConfig
            {
                Aps = new Aps
                {
                    Alert = new ApsAlert { Title = request.Title, Body = request.Body },
                    MutableContent = true,
                },
                FcmOptions = new ApnsFcmOptions { ImageUrl = request.ImageUrl },
            },
        };

        var messaging = FirebaseMessaging.DefaultInstance;

        var notiLog = new TblNotificationLog()
        {
            LogId = Ulid.NewUlid().ToString(),
            LogType = NotificationTypeConstant.PushNoti,
            Payload = request.ToJson(),
            CreatedAt = DateTime.Now,
        };
        await _unitOfWork.NotificationLogRepository.AddAsync(notiLog, cs);
        await _unitOfWork.SaveChangesAsync(cs);

        var response = await messaging.SendAsync(message, cs);

        notiLog.ResponseAt = DateTime.Now;
        notiLog.IsSuccess = true;
        notiLog.ResponseMessage = response;

        _unitOfWork.NotificationLogRepository.Update(notiLog);
        await _unitOfWork.SaveChangesAsync(cs);

        return BaseResponse<PushNotiResponse>.Success();
    }
}
