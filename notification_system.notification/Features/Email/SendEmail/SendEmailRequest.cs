using notification_system.notification.Models;

namespace notification_system.notification.Features.Email.SendEmail;

public class SendEmailRequest
{
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public string ToEmail { get; set; }
    public string? CcEmail { get; set; }
    public string? BccEmail { get; set; }
    public List<FileModel>? Files { get; set; }
}
