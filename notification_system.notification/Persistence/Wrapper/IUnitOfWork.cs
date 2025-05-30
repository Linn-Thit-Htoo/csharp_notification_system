using notification_system.notification.Features.NotificationLogs.Core;

namespace notification_system.notification.Persistence.Wrapper
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cs = default);
        INotificationLogRepository NotificationLogRepository { get; }
    }
}
