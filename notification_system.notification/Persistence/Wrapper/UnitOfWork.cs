using System.Reflection;
using System.Security.Claims;
using notification_system.notification.Entities;
using notification_system.notification.Features.NotificationLogs.Core;
using notification_system.notification.Features.Otp.Core;

namespace notification_system.notification.Persistence.Wrapper;

public class UnitOfWork : IUnitOfWork
{
    internal readonly DbContext _context;
    internal readonly string? _currentUser = "SYSTEM";
    internal readonly IHttpContextAccessor _httpContextAccessor;

    public UnitOfWork(IHttpContextAccessor httpContextAccessor, NotificationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = dbContext;

        if (_httpContextAccessor.HttpContext is not null)
        {
            if (_httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            {
                _currentUser = _httpContextAccessor
                    .HttpContext.User.Identities.FirstOrDefault()!
                    .Claims.Where(x => x.Type == ClaimTypes.NameIdentifier)
                    .FirstOrDefault()!
                    .Value;
            }
        }

        NotificationLogRepository = new NotificationLogRepository(_context);
        OtpRepository = new OtpRepository(_context);
    }

    public void SaveChanges()
    {
        var modifiedEntries = _context
            .ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            Type type = entry.Entity.GetType();

            if (entry.State == EntityState.Added)
            {
                PropertyInfo createdBy = type.GetProperty("CreatedBy")!;
                createdBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo createdDate = type.GetProperty("CreatedAt")!;
                createdDate?.SetValue(entry.Entity, DateTime.Now);
            }

            if (entry.State == EntityState.Modified)
            {
                PropertyInfo modifiedBy = type.GetProperty("ModifiedBy")!;
                modifiedBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo modifiedAt = type.GetProperty("ModifiedAt")!;
                modifiedAt?.SetValue(entry.Entity, DateTime.Now);
            }
        }

        _context.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cs = default)
    {
        var modifiedEntries = _context
            .ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            Type type = entry.Entity.GetType();

            if (entry.State == EntityState.Added)
            {
                PropertyInfo createdBy = type.GetProperty("CreatedBy")!;
                createdBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo createdDate = type.GetProperty("CreatedAt")!;
                createdDate?.SetValue(entry.Entity, DateTime.Now);
            }

            if (entry.State == EntityState.Modified)
            {
                PropertyInfo modifiedBy = type.GetProperty("ModifiedBy")!;
                modifiedBy?.SetValue(entry.Entity, _currentUser);

                PropertyInfo modifiedAt = type.GetProperty("ModifiedAt")!;
                modifiedAt?.SetValue(entry.Entity, DateTime.Now);
            }
        }

        await _context.SaveChangesAsync(cs);
    }

    public INotificationLogRepository NotificationLogRepository { get; set; }

    public IOtpRepository OtpRepository { get; set; }
}
