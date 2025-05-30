using Microsoft.EntityFrameworkCore;
using notification_system.notification.Entities;
using notification_system.notification.Persistence.Base;

namespace notification_system.notification.Features.Otp.Core
{
    public class OtpRepository : RepositoryBase<TblOtp>, IOtpRepository
    {
        public OtpRepository(DbContext context) : base(context)
        {
        }
    }
}
