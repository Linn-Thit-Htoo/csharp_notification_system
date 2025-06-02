using System;

namespace notification_system.notification.Entities;

public partial class TblNotificationLog
{
    public string LogId { get; set; } = null!;

    public string LogType { get; set; } = null!;

    public string? ToEmailList { get; set; }

    public string? CcEmailList { get; set; }

    public string? BccEmailList { get; set; }

    public string? ToPhoneList { get; set; }

    public string? Payload { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ResponseAt { get; set; }

    public bool? IsSuccess { get; set; }

    public string? ResponseMessage { get; set; }
}
