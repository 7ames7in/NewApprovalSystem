// using ApprovalService.Domain.Entities;
// using AttachmentService.Domain.Entities;
// using Microsoft.EntityFrameworkCore;

// namespace ApprovalService.Infrastructure.Persistence;

// public class ApprovalDbContext : DbContext
// {
//     public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options) : base(options) { }

//     public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
//     public DbSet<ApprovalStep> ApprovalSteps { get; set; }
//     public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }
//     public DbSet<ApprovalAttachment> ApprovalAttachments { get; set; }

//     // filepath: /Users/7ames7in/Documents/GitHub/New-Projects/NewApprovalSystem/src/Services/ApprovalService/Infrastructure/Persistence/ApprovalDbContext.cs
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);

//         modelBuilder.Entity<ApprovalStep>(entity =>
//         {
//             entity.HasKey(e => e.StepId); // Configure as keyless
//         });
//     }
// }

using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;

namespace ApprovalService.Infrastructure.Persistence
{
    public class ApprovalDbContext : DbContext
    {
        public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options) : base(options) { }

        public DbSet<Approval> Approvals { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<ApprovalStep> ApprovalSteps { get; set; }
        public DbSet<ApprovalTemplate> ApprovalTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ValueObject: Approver 설정
            modelBuilder.Entity<Approval>().OwnsOne(a => a.Approver, nav =>
            {
                nav.Property(p => p.EmployeeNumber).HasColumnName("ApproverEmployeeNumber").IsRequired();
                nav.Property(p => p.Name).HasColumnName("ApproverName");
                nav.Property(p => p.Position).HasColumnName("ApproverPosition");
                nav.Property(p => p.Department).HasColumnName("ApproverDepartment");
            });

            modelBuilder.Entity<Approval>().HasKey(a => a.Id);
            modelBuilder.Entity<ApprovalRequest>().HasKey(r => r.ApprovalId);
            modelBuilder.Entity<ApprovalStep>().HasKey(s => s.StepId);
            modelBuilder.Entity<ApprovalTemplate>().HasKey(t => t.TemplateId);
        }
    }
}
