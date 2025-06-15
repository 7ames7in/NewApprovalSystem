using Microsoft.EntityFrameworkCore;
using LoggingService.Domain.Entities;

namespace LoggingService.Infrastructure.Persistence
{
    public class SystemLogDbContext(DbContextOptions<SystemLogDbContext> options) : DbContext(options)
    {

        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogLevel).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.MessageTemplate).IsRequired();
                entity.Property(e => e.RenderedMessage).IsRequired();
                entity.Property(e => e.Level).IsRequired();
            });
            modelBuilder.Entity<EventLog>()
                .Property(e => e.Properties)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new Dictionary<string, object>());
            modelBuilder.Entity<EventLog>()
                .Property(e => e.Exception)
                .HasConversion(
                    v => v ?? string.Empty,
                    v => v);
            modelBuilder.Entity<EventLog>()
                .Property(e => e.LogId)
                .ValueGeneratedOnAdd(); // Ensure LogId is generated on add
            
        }
    }
}
