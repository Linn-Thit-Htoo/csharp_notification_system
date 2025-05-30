using System;
using System.Collections.Generic;

namespace notification_system.notification.Entities;

public partial class TblSmsTemplate
{
    public string Id { get; set; } = null!;

    public string BodyContent { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
