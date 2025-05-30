namespace notification_system.notification.Persistence.Wrapper
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cs = default);
    }
}
