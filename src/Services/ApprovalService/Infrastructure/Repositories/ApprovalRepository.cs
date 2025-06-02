using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Enums;

namespace ApprovalService.Infrastructure.Repositories;

public class ApprovalRepository<T> : IApprovalRepository<T> where T : ApprovalRequest
{
    private readonly ApprovalDbContext _context;

    public async Task<IEnumerable<T>> GetMyRejectedApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => EF.Property<string>(approval, "ApplicantEmployeeNumber") == userId && approval.Status == "Rejected")
            .ToListAsync();
    }

    public ApprovalRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetMyApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => approval.Steps.Any(step => step.ApproverEmployeeNumber == userId))
            .Include(approval => approval.Steps)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetMyPendingApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => EF.Property<string>(approval, "ApplicantEmployeeNumber") == userId && approval.Status == "Pending")
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetMyApprovedApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => EF.Property<string>(approval, "ApplicantEmployeeNumber") == userId && approval.Status == "Approved")
            .OrderByDescending(approval => approval.RequestedAt)
            .Include(approval => approval.Steps)
            //.ThenByDescending(step => step.Sequence)
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
