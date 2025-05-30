using System;
using System.Collections.Generic;

namespace notification_system.notification.Entities;

public partial class TblOtp
{
    public string OtpId { get; set; } = null!;

    public int OtpValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public bool IsDeleted { get; set; }
}
