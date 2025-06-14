using Microsoft.EntityFrameworkCore;
using notification_system.notification.Entities;

namespace notification_system.notification.Features.Otp.Core;

public class OtpRepository : RepositoryBase<TblOtp>, IOtpRepository
{
    public OtpRepository(DbContext context)
        : base(context) { }
}
