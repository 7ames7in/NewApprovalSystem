namespace LoggingService.Domain.Entities;

public class SystemLog
{
    public Guid LogId { get; set; }
    public string LogLevel { get; set; }
    public string Message { get; set; }
    public string? Source { get; set; }
    public DateTime CreatedAt { get; set; }
}
