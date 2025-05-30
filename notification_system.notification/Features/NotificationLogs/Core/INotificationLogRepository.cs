using notification_system.notification.Entities;
using notification_system.notification.Persistence.Base;

namespace notification_system.notification.Features.NotificationLogs.Core
{
    public interface INotificationLogRepository : IRepositoryBase<TblNotificationLog>
    {
    }
}
