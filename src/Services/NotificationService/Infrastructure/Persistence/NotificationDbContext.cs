using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmailNotification> EmailNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);
                entity.Property(e => e.Recipient).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Subject).HasMaxLength(300);
                entity.Property(e => e.Body).IsRequired(false);
                entity.Property(e => e.SentAt).IsRequired(false);
                entity.Property(e => e.Status).HasMaxLength(50);
            });
        }
    }
}
