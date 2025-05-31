using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using notification_system.notification.Configurations;
using notification_system.notification.Extensions;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Features.PushNoti;
using notification_system.notification.Persistence.Wrapper;
using notification_system.notification.Services.EmailServices;
using notification_system.notification.Services.PushNoti;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace notification_system.notification.Services.RabbitMQ
{
    public class RabbitMQService : BackgroundService
    {
        private readonly AppSetting _setting;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<RabbitMQService> _logger;

        public RabbitMQService(IOptions<AppSetting> setting, IServiceScopeFactory serviceScopeFactory, ILogger<RabbitMQService> logger)
        {
            _setting = setting.Value;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                IConnection connection = CreateConnection();
                var channel = connection.CreateModel();

                foreach (Queuelist item in _setting.RabbitMQ.QueueList)
                {
                    channel.ExchangeDeclare(item.Exchange, ExchangeType.Direct, true, false, null);
                    channel.QueueDeclare(item.Queue, true, false, false);
                    channel.QueueBind(item.Queue, item.Exchange, item.RoutingKey, null);
                    channel.BasicQos(0, 1, false);
                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += async (ch, ea) =>
                    {
                        var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var serviceProvider = _serviceScopeFactory.CreateScope().ServiceProvider;
                        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
                        var emailService = serviceProvider.GetRequiredService<IEmailService>();
                        var pushNotiService = serviceProvider.GetRequiredService<IPushNotiService>();

                        if (item.RoutingKey.Equals("single_email_direct"))
                        {
                            var requestModel = content.ToObject<SendEmailRequest>();
                            await emailService.SendEmailAsync(requestModel);
                        }

                        if (item.RoutingKey.Equals("multiple_email_direct"))
                        {
                            var requestModel = content.ToObject<SendEmailMultipleRequest>();
                            await emailService.SendMultipleEmailAysnc(requestModel);
                        }

                        if (item.RoutingKey.Equals("pushnoti_direct"))
                        {
                            var requestModel = content.ToObject<PushNotiRequest>();
                            await pushNotiService.PushNotiAsync(requestModel);
                        }

                        channel.BasicAck(ea.DeliveryTag, false);
                    };

                    channel.BasicConsume(item.Queue, false, consumer);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitMQ background service error: {ex.ToString()}");
            }
        }

        private IConnection CreateConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = _setting.RabbitMQ.HostName,
                UserName = _setting.RabbitMQ.UserName,
                Password = _setting.RabbitMQ.Password,
                VirtualHost = "/"
            };
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(15);
            connectionFactory.DispatchConsumersAsync = true;

            return connectionFactory.CreateConnection();
        }
    }
}
