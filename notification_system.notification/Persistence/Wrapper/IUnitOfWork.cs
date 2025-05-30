using notification_system.notification.Features.NotificationLogs.Core;
using notification_system.notification.Features.Otp.Core;

namespace notification_system.notification.Persistence.Wrapper
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cs = default);
        INotificationLogRepository NotificationLogRepository { get; }
        IOtpRepository OtpRepository { get; }
    }
}
