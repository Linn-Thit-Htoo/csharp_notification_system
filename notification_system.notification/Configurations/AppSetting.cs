namespace notification_system.notification.Configurations
{
    public class AppSetting
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
        public Sendgrid SendGrid { get; set; }
        public Otpconfig OtpConfig { get; set; }
        public Rabbitmq RabbitMQ { get; set; }
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
}
