
namespace notification_system.notification.Features.NotificationLogs.Core;

public class NotificationLogRepository
    : RepositoryBase<TblNotificationLog>,
        INotificationLogRepository
{
    public NotificationLogRepository(DbContext context)
        : base(context) { }
}
