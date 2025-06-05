using Confluent.Kafka;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Extensions;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Services.EmailServices;
using static notification_system.notification.Extensions.Extension;

namespace notification_system.notification.Services.Kafka;

public class MultipleEmailConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<MultipleEmailConsumerService> _logger;
    private readonly AppSetting _appSetting;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MultipleEmailConsumerService(ILogger<MultipleEmailConsumerService> logger, IOptions<AppSetting> setting, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _appSetting = setting.Value;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _appSetting.Kafka.BootstrapServers,
            GroupId = _appSetting.Kafka.Email.MultipleEmail.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Extension.EnsureTopicExistsAsync(_appSetting.Kafka.BootstrapServers, _appSetting.Kafka.Email.MultipleEmail.Topic);
        _consumer.Subscribe(_appSetting.Kafka.Email.MultipleEmail.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = _serviceScopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var request = message.ToObject<SendMultipleEmailRequest>();

                await emailService.SendMultipleEmailAysnc(request, stoppingToken);
                _logger.LogInformation($"Received Multiple Email Consumer: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Multiple Email Consumer: {ex.Message}");
            }
        }
    }
}
