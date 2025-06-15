namespace notification_system.notification.Services.SMSServices;

public class TwilioService : ITwilioService
{
    private readonly AppSetting _setting;
    private readonly ILogger<TwilioService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public TwilioService(
        IOptions<AppSetting> setting,
        ILogger<TwilioService> logger,
        IUnitOfWork unitOfWork
    )
    {
        _setting = setting.Value;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SendMultipleSMSAsync(
        SendMultipleSMSRequest request,
        CancellationToken cs = default
    )
    {
        try
        {
            TwilioClient.Init(_setting.Twilio.AccountSid, _setting.Twilio.AuthToken);

            var notiLog = new TblNotificationLog()
            {
                LogId = Ulid.NewUlid().ToString(),
                ToPhoneList = request.ToPhoneNumbers.ToJson(),
                Payload = request.Messasge,
                CreatedAt = DateTime.Now,
                LogType = NotificationTypeConstant.SMS,
            };

            await _unitOfWork.NotificationLogRepository.AddAsync(notiLog, cs);
            await _unitOfWork.SaveChangesAsync(cs);

            foreach (var item in request.ToPhoneNumbers)
            {
                var message = await MessageResource.CreateAsync(
                    body: request.Messasge,
                    from: new PhoneNumber(_setting.Twilio.FromPhoneNumber),
                    to: new PhoneNumber(item)
                );
            }

            notiLog.ResponseAt = DateTime.Now;
            notiLog.IsSuccess = true;

            _unitOfWork.NotificationLogRepository.Update(notiLog);
            await _unitOfWork.SaveChangesAsync(cs);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Send Multiple SMS Error: {ex.ToString()}");
        }
    }

    public async Task SendSingleSMSAsync(
        SendSingleSMSRequest request,
        CancellationToken cs = default
    )
    {
        try
        {
            TwilioClient.Init(_setting.Twilio.AccountSid, _setting.Twilio.AuthToken);

            var notiLog = new TblNotificationLog()
            {
                LogId = Ulid.NewUlid().ToString(),
                ToPhoneList = request.ToPhoneNumber.ToJson(),
                Payload = request.Messasge,
                CreatedAt = DateTime.Now,
                LogType = NotificationTypeConstant.SMS,
            };
            await _unitOfWork.NotificationLogRepository.AddAsync(notiLog, cs);
            await _unitOfWork.SaveChangesAsync(cs);

            var message = await MessageResource.CreateAsync(
                body: request.Messasge,
                from: new PhoneNumber(_setting.Twilio.FromPhoneNumber),
                to: new PhoneNumber(request.ToPhoneNumber)
            );

            notiLog.ResponseAt = DateTime.Now;
            notiLog.IsSuccess = true;
            notiLog.ResponseMessage = message.Body;

            _unitOfWork.NotificationLogRepository.Update(notiLog);
            await _unitOfWork.SaveChangesAsync(cs);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Send Single SMS Error: {ex.ToString()}");
        }
    }
}
