using System;
using LoggingService.Infrastructure.Dtos;

namespace LoggingService.Infrastructure.Interfaces;

public interface ILoggingRepository
{
    Task SaveLogAsync(LogEventDto log);
    Task<IEnumerable<LogEventDto>> GetLogsAsync(DateTime? from = null, DateTime? to = null, string? level = null);
    Task<LogEventDto?> GetLogByIdAsync(Guid id);
    Task DeleteLogAsync(Guid id);
    Task ClearLogsAsync();
}