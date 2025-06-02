using Microsoft.EntityFrameworkCore;

namespace notification_system.notification.Entities;

public partial class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options) { }

    public virtual DbSet<TblEmailTemplate> TblEmailTemplates { get; set; }

    public virtual DbSet<TblNotificationLog> TblNotificationLogs { get; set; }

    public virtual DbSet<TblOtp> TblOtps { get; set; }

    public virtual DbSet<TblSmsTemplate> TblSmsTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblEmailTemplate>(entity =>
        {
            entity.ToTable("Tbl_Email_Templates");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.BccEmailList).HasColumnName("BCcEmailList");
            entity.Property(e => e.ContentType).HasMaxLength(10).HasComment("Rich Text, HTML");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(150);
            entity.Property(e => e.TemplateName).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblNotificationLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("Tbl_Notification_Logs");

            entity.Property(e => e.LogId).HasMaxLength(50);
            entity.Property(e => e.BccEmailList).HasColumnName("BCcEmailList");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LogType).HasMaxLength(10);
            entity.Property(e => e.ResponseAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblOtp>(entity =>
        {
            entity.HasKey(e => e.OtpId);

            entity.ToTable("Tbl_Otp");

            entity.Property(e => e.OtpId).HasMaxLength(50);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblSmsTemplate>(entity =>
        {
            entity.ToTable("Tbl_SMS_Templates");

            entity.Property(e => e.Id).HasMaxLength(10).IsFixedLength();
            entity.Property(e => e.BodyContent).HasMaxLength(300);
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
