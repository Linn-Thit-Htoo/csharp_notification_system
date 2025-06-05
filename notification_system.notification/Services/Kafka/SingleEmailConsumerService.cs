using Confluent.Kafka;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Extensions;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Services.EmailServices;
using static notification_system.notification.Extensions.Extension;

namespace notification_system.notification.Services.Kafka
{
    public class SingleEmailConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<SingleEmailConsumerService> _logger;
        private readonly AppSetting _appSetting;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SingleEmailConsumerService(ILogger<SingleEmailConsumerService> logger, IOptions<AppSetting> setting, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _appSetting = setting.Value;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _appSetting.Kafka.BootstrapServers,
                GroupId = _appSetting.Kafka.Email.SingleEmail.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Extension.EnsureTopicExistsAsync(_appSetting.Kafka.BootstrapServers, _appSetting.Kafka.Email.SingleEmail.Topic);
            _consumer.Subscribe(_appSetting.Kafka.Email.SingleEmail.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var scope = _serviceScopeFactory.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    var consumeResult = _consumer.Consume(stoppingToken);
                    var message = consumeResult.Message.Value;
                    var request = message.ToObject<SendEmailRequest>();

                    await emailService.SendEmailAsync(request, stoppingToken);
                    _logger.LogInformation($"Received Single Email Consumer: {message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing Single Email Consumer: {ex.Message}");
                }
            }
        }
    }
}
