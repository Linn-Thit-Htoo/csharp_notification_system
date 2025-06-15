using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Extensions;
using notification_system.notification.Features.SMS.SendSMS;
using notification_system.notification.Services.SMSServices;
using static notification_system.notification.Extensions.Extension;

namespace notification_system.notification.Services.Kafka;

public class MultipleSMSConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<MultipleSMSConsumerService> _logger;
    private readonly AppSetting _appSetting;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MultipleSMSConsumerService(
        ILogger<MultipleSMSConsumerService> logger,
        IOptions<AppSetting> setting,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _appSetting = setting.Value;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _appSetting.Kafka.BootstrapServers,
            GroupId = _appSetting.Kafka.SMS.MultipleSMS.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Extension.EnsureTopicExistsAsync(
            _appSetting.Kafka.BootstrapServers,
            _appSetting.Kafka.SMS.MultipleSMS.Topic
        );
        _consumer.Subscribe(_appSetting.Kafka.SMS.MultipleSMS.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = _serviceScopeFactory.CreateScope();
                var smsService = scope.ServiceProvider.GetRequiredService<ITwilioService>();
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var request = message.ToObject<SendMultipleSMSRequest>();

                await smsService.SendMultipleSMSAsync(request, stoppingToken);
                _logger.LogInformation($"Received Single SMS Consumer: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Single SMS Consumer: {ex.Message}");
            }
        }
    }
}
