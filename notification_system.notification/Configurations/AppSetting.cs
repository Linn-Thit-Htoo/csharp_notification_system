namespace notification_system.notification.Configurations
{
    public class AppSetting
    {
        public Connectionstrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
        public Sendgrid SendGrid { get; set; }
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
}
