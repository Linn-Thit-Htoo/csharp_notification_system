namespace notification_system.gateway.Configurations;

public class AppSetting
{
    public Logging Logging { get; set; }
    public Consul Consul { get; set; }
}

public class Logging
{
    public Loglevel LogLevel { get; set; }
}

public class Loglevel
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class Consul
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int Port { get; set; }
    public string DiscoveryAddress { get; set; }
    public string HealthCheckEndPoint { get; set; }
}
