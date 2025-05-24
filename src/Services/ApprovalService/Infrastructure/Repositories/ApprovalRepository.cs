using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Repositories;

public class ApprovalRepository : IApprovalRepository
{
    private readonly ApprovalDbContext _context;

    public ApprovalRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Approval approval)
    {
        await _context.Approvals.AddAsync(approval);
    }

    public async Task<Approval?> GetByIdAsync(Guid id)
    {
        return await _context.Approvals.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
