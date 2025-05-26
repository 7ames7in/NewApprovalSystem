using ApprovalService.Domain.Entities;
using AttachmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Persistence;

public class ApprovalDbContext : DbContext
{
    public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options) : base(options) { }

    public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
    public DbSet<ApprovalStep> ApprovalSteps { get; set; }
    public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }
    public DbSet<ApprovalAttachment> ApprovalAttachments { get; set; }
}