public class EmailNotification
{
    public Guid EmailNotificationId { get; set; }
    public string Recipient { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string? Body { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; } = "NotReady"; // e.g., "Sent", "Failed", "Pending"
}
