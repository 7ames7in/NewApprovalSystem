namespace LoggingService.Domain.Entities;

public class EventLog
{
    public Guid LogId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; }
    public string MessageTemplate { get; set; } = string.Empty;
    public string RenderedMessage { get; set; } = string.Empty;
    public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    public string Level { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
}