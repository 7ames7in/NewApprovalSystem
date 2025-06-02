using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Repositories;

public class ApprovalRequestRepository<T> : IApprovalRequestRepository<T> where T : ApprovalRequest
{
    private readonly ApprovalDbContext _context;

    public async Task<T> CreateApprovalRequestAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public ApprovalRequestRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetMyRequestsAsync(string userId)
    {
        // Implement logic to fetch requests for the given userId
            return await _context.Set<T>()
                .Where(request => EF.Property<string>(request, "ApplicantEmployeeNumber") == userId) // Assuming T has a UserId property
                .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetMyPendingRequestsAsync(string userId)
    {
        // Implement logic to fetch pending requests for the given userId
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetMyApprovedRequestsAsync(string userId)
    {
        // Implement logic to fetch approved requests for the given userId
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetMyRejectedRequestsAsync(string userId)
    {
        // Implement logic to fetch rejected requests for the given userId
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetRequestByIdAsync(Guid id)
    {   
        // Implement logic to fetch a request by its ID
        var entity = await _context.Set<T>()
            .Include(p => p.Steps)
            .SingleOrDefaultAsync(request => request.ApprovalId == id);
        
        //entity = await _context.Set<T>().FindAsync(id);
        return entity;
    }

    public async Task ApproveRequestAsync(Guid id)
    {
        // Implement logic to approve a request by its ID
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            // Update entity to mark it as approved
            await _context.SaveChangesAsync();
        }
    }

    public async Task RejectRequestAsync(Guid id)
    {
        // Implement logic to reject a request by its ID
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            // Update entity to mark it as rejected
            await _context.SaveChangesAsync();
        }
    }


    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
