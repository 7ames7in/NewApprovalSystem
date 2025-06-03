using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Enums;

namespace ApprovalService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for managing ApprovalRequest entities.
/// </summary>
/// <typeparam name="T">The type of ApprovalRequest.</typeparam>
public class ApprovalRepository<T> : IApprovalRepository<T> where T : ApprovalRequest
{
    private readonly ApprovalDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ApprovalRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ApprovalRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all rejected approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user's employee number.</param>
    /// <returns>A list of rejected approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyRejectedApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => EF.Property<string>(approval, "ApplicantEmployeeNumber") == userId && approval.Status == "Rejected")
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all approval requests where the user is an approver.
    /// </summary>
    /// <param name="userId">The user's employee number.</param>
    /// <returns>A list of approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => approval.Steps.Any(step => step.ApproverEmployeeNumber == userId))
            .Include(approval => approval.Steps)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all pending approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user's employee number.</param>
    /// <returns>A list of pending approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyPendingApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => EF.Property<string>(approval, "ApplicantEmployeeNumber") == userId && approval.Status == "Pending")
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all approved approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user's employee number.</param>
    /// <returns>A list of approved approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyApprovedApprovalRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(approval => EF.Property<string>(approval, "ApplicantEmployeeNumber") == userId && approval.Status == "Approved")
            .OrderByDescending(approval => approval.RequestedAt)
            .Include(approval => approval.Steps)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new approval request entity to the database.
    /// </summary>
    /// <param name="entity">The approval request entity.</param>
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    /// <summary>
    /// Retrieves an approval request entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the approval request.</param>
    /// <returns>The approval request entity, or null if not found.</returns>
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Retrieves all approval request entities.
    /// </summary>
    /// <returns>A list of all approval request entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Updates an existing approval request entity.
    /// </summary>
    /// <param name="entity">The approval request entity to update.</param>
    public Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes an approval request entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the approval request to delete.</param>
    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
        }
    }

    /// <summary>
    /// Saves changes made to the database context.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
