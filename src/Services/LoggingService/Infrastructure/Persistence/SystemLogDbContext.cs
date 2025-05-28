using Microsoft.EntityFrameworkCore;
using LoggingService.Domain.Entities;

namespace LoggingService.Infrastructure.Persistence
{
    public class SystemLogDbContext : DbContext
    {
        public SystemLogDbContext(DbContextOptions<SystemLogDbContext> options) : base(options) { }

        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.LogLevel).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Source).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
