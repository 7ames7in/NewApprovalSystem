using System;

namespace LoggingService.Infrastructure.Dtos;

public class LogEventDto
{
    public DateTime Timestamp { get; set; }
    public string MessageTemplate { get; set; } = string.Empty;
    public string RenderedMessage { get; set; } = string.Empty;
    public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    public string Level { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
}