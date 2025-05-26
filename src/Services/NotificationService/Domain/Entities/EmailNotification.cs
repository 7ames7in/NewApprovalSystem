public class EmailNotification
{
    public Guid EmailNotificationId { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string? Body { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; }
}
