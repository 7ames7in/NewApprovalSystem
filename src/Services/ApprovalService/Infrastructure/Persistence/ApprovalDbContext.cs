using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;

namespace ApprovalService.Infrastructure.Persistence
{
    public class ApprovalDbContext : DbContext
    {
        // Constructor to initialize DbContext with options
        public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options) : base(options) { }

        // DbSet properties for each entity
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<ApprovalStep> ApprovalSteps { get; set; }
        public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }
        public DbSet<ApprovalAttachment> ApprovalAttachments { get; set; }

        // Configuring entity relationships and keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys for entities
            modelBuilder.Entity<ApprovalRequest>().HasKey(r => r.ApprovalId); // ApprovalRequest primary key
            modelBuilder.Entity<ApprovalStep>().HasKey(s => s.StepId);       // ApprovalStep primary key
            modelBuilder.Entity<ApprovalTemplate>().HasKey(t => t.TemplateId); // ApprovalTemplate primary key
            modelBuilder.Entity<ApprovalAttachment>().HasKey(a => a.AttachmentId); // ApprovalAttachment primary key

            // Define relationships between entities

            // ApprovalRequest 1:N ApprovalSteps
            modelBuilder.Entity<ApprovalRequest>()
                .HasMany(r => r.Steps) // ApprovalRequest has many ApprovalSteps
                .WithOne(s => s.ApprovalRequest) // ApprovalStep references ApprovalRequest
                .HasForeignKey(s => s.ApprovalId) // Foreign key in ApprovalStep
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            // ApprovalRequest 1:N ApprovalAttachments
            modelBuilder.Entity<ApprovalRequest>()
                .HasMany(r => r.Attachments) // ApprovalRequest has many ApprovalAttachments
                .WithOne(a => a.ApprovalRequest) // ApprovalAttachment references ApprovalRequest
                .HasForeignKey(a => a.ApprovalId) // Foreign key in ApprovalAttachment
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior
        }
    }
}
