namespace notification_system.notification.Models;

public class FileModel
{
    public string FileName { get; set; }
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
}
