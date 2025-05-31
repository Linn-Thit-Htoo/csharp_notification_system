using Consul;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;

namespace notification_system.notification.Services.ServiceDiscovery
{
    public class ConsulService : IHostedService
    {
        private readonly IConsulClient _client;
        private AgentServiceRegistration _registration;
        private readonly AppSetting _setting;

        public ConsulService(IConsulClient client, IOptions<AppSetting> setting)
        {
            _client = client;
            _setting = setting.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _registration = new AgentServiceRegistration
            {
                ID = _setting.Consul.Id,
                Name = _setting.Consul.Name,
                Address = _setting.Consul.Address,
                Port = _setting.Consul.Port,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = _setting.Consul.HealthCheckEndPoint,
                    Timeout = TimeSpan.FromSeconds(10),
                },
            };

            await _client
                .Agent.ServiceDeregister(_registration.ID, cancellationToken)
                .ConfigureAwait(false);

            await _client.Agent.ServiceRegister(_registration, cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var registration = new AgentServiceRegistration { ID = _setting.Consul.Id };

            await _client
                .Agent.ServiceDeregister(_registration.ID, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
