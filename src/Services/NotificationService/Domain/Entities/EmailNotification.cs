namespace NotificationService.Domain.Entities;

public class EmailNotification
{
    public Guid NotificationId { get; set; }
    public string Recipient { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string? Body { get; set; }
    public DateTime? SentAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "NotReady"; // e.g., "Sent", "Failed", "Pending"
}
