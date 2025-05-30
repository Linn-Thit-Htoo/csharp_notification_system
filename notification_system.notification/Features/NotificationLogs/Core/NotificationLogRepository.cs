using Microsoft.EntityFrameworkCore;
using notification_system.notification.Entities;
using notification_system.notification.Persistence.Base;

namespace notification_system.notification.Features.NotificationLogs.Core
{
    public class NotificationLogRepository : RepositoryBase<TblNotificationLog>, INotificationLogRepository
    {
        public NotificationLogRepository(DbContext context) : base(context)
        {
        }
    }
}
