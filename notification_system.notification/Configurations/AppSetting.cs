namespace notification_system.notification.Configurations;

public class AppSetting
{
    public Connectionstrings ConnectionStrings { get; set; }
    public Logging Logging { get; set; }
    public Sendgrid SendGrid { get; set; }
    public Otpconfig OtpConfig { get; set; }
    public Rabbitmq RabbitMQ { get; set; }
    public Kafka Kafka { get; set; }
    public Consul Consul { get; set; }
    public Twilio Twilio { get; set; }
}

public class Connectionstrings
{
    public string NotiConnection { get; set; }
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

public class Sendgrid
{
    public string API_KEY { get; set; }
    public string FROM_NAME { get; set; }
    public string FROM_EMAIL { get; set; }
}

public class Otpconfig
{
    public int ExpireInMinutes { get; set; }
}

public class Rabbitmq
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Queuelist[] QueueList { get; set; }
}

public class Queuelist
{
    public string Exchange { get; set; }
    public string Queue { get; set; }
    public string RoutingKey { get; set; }
}

public class Kafka
{
    public string BootstrapServers { get; set; }
    public Email Email { get; set; }
    public SMS SMS { get; set; }
}

public class Email
{
    public Singleemail SingleEmail { get; set; }
    public Multipleemail MultipleEmail { get; set; }
}

public class Singleemail
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
}

public class Multipleemail
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
}

public class SMS
{
    public Singlesms SingleSMS { get; set; }
    public Multiplesms MultipleSMS { get; set; }
}

public class Singlesms
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
}

public class Multiplesms
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
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

public class Twilio
{
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string FromPhoneNumber { get; set; }
}
