using ApprovalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Persistence;

public class ApprovalDbContext : DbContext
{
    public ApprovalDbContext(DbContextOptions<ApprovalDbContext> options)
        : base(options) { }

    public DbSet<Approval> Approvals => Set<Approval>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Approval>().OwnsOne(a => a.Approver);
        base.OnModelCreating(modelBuilder);
    }
}
