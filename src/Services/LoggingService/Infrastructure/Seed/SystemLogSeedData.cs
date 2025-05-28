using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoggingService.Domain.Entities;
using LoggingService.Infrastructure.Persistence;

namespace LoggingService.Infrastructure.Seed
{
    public static class SystemLogSeedData
    {
        public static async Task InitializeAsync(SystemLogDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.SystemLogs.AnyAsync())
                return;

            var logs = new[]
            {
                new SystemLog
                {
                    LogId = Guid.NewGuid(),
                    LogLevel = "Information",
                    Message = "Application started successfully.",
                    Source = "Startup",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30)
                },
                new SystemLog
                {
                    LogId = Guid.NewGuid(),
                    LogLevel = "Warning",
                    Message = "Disk space is running low.",
                    Source = "HealthCheck",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new SystemLog
                {
                    LogId = Guid.NewGuid(),
                    LogLevel = "Error",
                    Message = "Unhandled exception occurred.",
                    Source = "Middleware",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.SystemLogs.AddRangeAsync(logs);
            await context.SaveChangesAsync();
        }
    }
}
