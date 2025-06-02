
namespace notification_system.notification.Entities;

public partial class TblEmailTemplate
{
    public string Id { get; set; } = null!;

    public string TemplateName { get; set; } = null!;

    public string Subject { get; set; } = null!;

    /// <summary>
    /// Rich Text, HTML
    /// </summary>
    public string ContentType { get; set; } = null!;

    public string BodyContent { get; set; } = null!;

    public string? CcEmailList { get; set; }

    public string? BccEmailList { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
