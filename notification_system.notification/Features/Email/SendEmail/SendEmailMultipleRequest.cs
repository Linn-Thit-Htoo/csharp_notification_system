using notification_system.notification.Models;

namespace notification_system.notification.Features.Email.SendEmail;

public class SendEmailMultipleRequest
{
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public List<string>? ToEmails { get; set; }
    public List<string>? CcEmails { get; set; }
    public List<string>? BccEmails { get; set; }
    public List<FileModel>? Files { get; set; }
}
