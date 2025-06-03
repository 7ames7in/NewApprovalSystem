using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;

namespace ApprovalService.Infrastructure.Persistence
{
    public class ApprovalDbContext : DbContext
    {
        public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options) : base(options) { }

        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<ApprovalStep> ApprovalSteps { get; set; }
        public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }
        public DbSet<ApprovalAttachment> ApprovalAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalRequest>().HasKey(r => r.ApprovalId);
            modelBuilder.Entity<ApprovalStep>().HasKey(s => s.StepId);
            modelBuilder.Entity<ApprovalTemplate>().HasKey(t => t.TemplateId);
            modelBuilder.Entity<ApprovalAttachment>().HasKey(a => a.AttachmentId); // Assuming AttachmentId is the key

            // ✅ Relationship: ApprovalRequest 1:N ApprovalSteps
            modelBuilder.Entity<ApprovalRequest>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.ApprovalRequest)
                .HasForeignKey(s => s.ApprovalId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Relationship: ApprovalRequest 1:N ApprovalAttachments
            modelBuilder.Entity<ApprovalRequest>()
                .HasMany(r => r.Attachments)
                .WithOne(a => a.ApprovalRequest)          // ApprovalAttachment Navigation Property
                .HasForeignKey(a => a.ApprovalId)
                .OnDelete(DeleteBehavior.Cascade);        // ApprovalRequest deletion cascades to Attachments
        }
    }
}
