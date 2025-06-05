using Confluent.Kafka;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Extensions;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Features.SMS.SendSMS;
using notification_system.notification.Services.EmailServices;
using notification_system.notification.Services.SMSServices;
using static notification_system.notification.Extensions.Extension;

namespace notification_system.notification.Services.Kafka
{
    public class SingleSMSConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<SingleSMSConsumerService> _logger;
        private readonly AppSetting _appSetting;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SingleSMSConsumerService(ILogger<SingleSMSConsumerService> logger, IOptions<AppSetting> setting, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _appSetting = setting.Value;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _appSetting.Kafka.BootstrapServers,
                GroupId = _appSetting.Kafka.SMS.SingleSMS.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Extension.EnsureTopicExistsAsync(_appSetting.Kafka.BootstrapServers, _appSetting.Kafka.SMS.SingleSMS.Topic);
            _consumer.Subscribe(_appSetting.Kafka.SMS.SingleSMS.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var scope = _serviceScopeFactory.CreateScope();
                    var smsService = scope.ServiceProvider.GetRequiredService<ITwilioService>();
                    var consumeResult = _consumer.Consume(stoppingToken);
                    var message = consumeResult.Message.Value;
                    var request = message.ToObject<SendSingleSMSRequest>();

                    await smsService.SendSingleSMSAsync(request, stoppingToken);
                    _logger.LogInformation($"Received Single SMS Consumer: {message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing Single SMS Consumer: {ex.Message}");
                }
            }
        }
    }
}
