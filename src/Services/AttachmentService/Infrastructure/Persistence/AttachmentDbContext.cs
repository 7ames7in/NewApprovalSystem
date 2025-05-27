// filepath: /Users/7ames7in/Documents/GitHub/New-Projects/NewApprovalSystem/src/Services/AttachmentService/Infrastructure/Persistence/AttachmentDbContext.cs
using AttachmentService.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace AttachmentService.Infrastructure.Persistence;

public class AttachmentDbContext : DbContext
{
    public AttachmentDbContext(DbContextOptions<AttachmentDbContext> options) : base(options) { }

    public DbSet<ApprovalAttachment> ApprovalAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly configure the primary key if necessary
        modelBuilder.Entity<ApprovalAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId); // Ensure the primary key is defined
        });
    }
}