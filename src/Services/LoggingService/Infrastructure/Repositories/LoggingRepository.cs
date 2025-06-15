using System;
using LoggingService.Infrastructure.Interfaces;
using LoggingService.Infrastructure.Dtos;
using LoggingService.Infrastructure.Persistence;
using LoggingService.Domain.Entities;

namespace LoggingService.Infrastructure.Repositories;

public class LoggingRepository : ILoggingRepository
{
    private readonly SystemLogDbContext _context;
    public LoggingRepository(SystemLogDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public Task SaveLogAsync(LogEventDto log)
    {
        // Implementation for saving a log
        if (log == null) throw new ArgumentNullException(nameof(log));

        _context.Add(new EventLog
        {
            Timestamp = log.Timestamp,
            MessageTemplate = log.MessageTemplate,
            RenderedMessage = log.RenderedMessage,
            Properties = log.Properties,
            Level = log.Level,
            Exception = log.Exception
        });
        return _context.SaveChangesAsync(); // Assuming SaveChangesAsync is available in the context
    }

    public Task<IEnumerable<LogEventDto>> GetLogsAsync(DateTime? from = null, DateTime? to = null, string? level = null)
    {
        // Implementation for retrieving logs
        throw new NotImplementedException();
    }

    public Task<LogEventDto?> GetLogByIdAsync(Guid id)
    {
        // Implementation for retrieving a log by ID
        throw new NotImplementedException();
    }

    public Task DeleteLogAsync(Guid id)
    {
        // Implementation for deleting a log
        throw new NotImplementedException();
    }

    public Task ClearLogsAsync()
    {
        // Implementation for clearing all logs
        throw new NotImplementedException();
    }
}
