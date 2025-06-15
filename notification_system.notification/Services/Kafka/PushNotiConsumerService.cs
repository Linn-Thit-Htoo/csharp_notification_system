
namespace notification_system.notification.Services.Kafka;

public class PushNotiConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<MultipleSMSConsumerService> _logger;
    private readonly AppSetting _appSetting;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PushNotiConsumerService(
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
            GroupId = _appSetting.Kafka.PushNotification.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Extension.EnsureTopicExistsAsync(
            _appSetting.Kafka.BootstrapServers,
            _appSetting.Kafka.PushNotification.Topic
        );
        _consumer.Subscribe(_appSetting.Kafka.PushNotification.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = _serviceScopeFactory.CreateScope();
                var pushNotiService = scope.ServiceProvider.GetRequiredService<IPushNotiService>();
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var request = message.ToObject<PushNotiRequest>();

                await pushNotiService.PushNotiAsync(request, stoppingToken);
                _logger.LogInformation($"Received Push Noti Consumer: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Push Noti Consumer: {ex.Message}");
            }
        }
    }
}
