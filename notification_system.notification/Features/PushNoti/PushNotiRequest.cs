using System.ComponentModel.DataAnnotations;

namespace notification_system.notification.Features.PushNoti;

public class PushNotiRequest
{
    public string DeviceToken { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? DeepLink { get; set; }
}
