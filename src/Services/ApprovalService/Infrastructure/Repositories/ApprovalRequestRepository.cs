using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using BuildingBlocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Repositories;

/// <summary>
/// Repository for managing approval requests.
/// </summary>
public class ApprovalRequestRepository<T> : IApprovalRequestRepository<T> where T : ApprovalRequestWithCurrentStepDto
{
    private readonly ApprovalDbContext _context;

    /// <summary>
    /// Initializes a new instance of the repository with the given database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ApprovalRequestRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new approval request.
    /// </summary>
    /// <param name="entity">The approval request entity.</param>
    /// <returns>The created approval request.</returns>
    public async Task<T> CreateApprovalRequestAsync(T entity)
    {
        var approvalRequest = entity as ApprovalRequest;
        if (approvalRequest == null)
        {
            throw new InvalidOperationException("Entity must inherit from ApprovalRequest.");
        }

        await _context.AddAsync(approvalRequest);
        await _context.SaveChangesAsync();
        return entity;
    }

    public IQueryable<ApprovalRequestWithCurrentStepDto> GetApprovalRequestsWithCurrentStep(string userId)
    {
        return _context.ApprovalRequests
            .Where(r => r.ApplicantEmployeeNumber == userId)
            .OrderByDescending(r => r.RequestedAt)
            .Select(r => new ApprovalRequestWithCurrentStepDto
            {
                ApprovalId = r.ApprovalId,
                RequestTitle = r.RequestTitle,
                RequestedAt = r.RequestedAt,
                RespondedAt = r.RespondedAt,
                ApplicantEmployeeNumber = r.ApplicantEmployeeNumber,
                ApplicantName = r.ApplicantName,
                ApplicantDepartment = r.ApplicantDepartment,
                ApplicantPosition = r.ApplicantPosition,
                Steps = r.Steps,
                Attachments = r.Attachments,
                Status = r.Status,
                CurrentStep = r.CurrentStep,
                CurrentApproverName = r.Steps
                    .Where(s => s.Sequence == r.CurrentStep)
                    .Select(s => s.ApproverName)
                    .FirstOrDefault(),
                CurrentApproverEmployeeNumber = r.Steps
                    .Where(s => s.Sequence == r.CurrentStep)
                    .Select(s => s.ApproverEmployeeNumber)
                    .FirstOrDefault(),
                CurrentActionStatus = r.Steps
                    .Where(s => s.Sequence == r.CurrentStep)
                    .Select(s => s.ActionStatus)
                    .FirstOrDefault()
            });
    }

    /// <summary>
    /// Retrieves all approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyRequestsAsync(string userId)
    {
        return await _context.Set<T>()
            .Where(request => EF.Property<string>(request, "ApplicantEmployeeNumber") == userId) // Assuming T has a UserId property
            .Include(request => request.Steps) // Include related steps if necessary
            .OrderByDescending(request => request.RequestedAt) // Assuming T has a CreatedAt property
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all pending approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of pending approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyPendingRequestsAsync(string userId)
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Retrieves all approved approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of approved approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyApprovedRequestsAsync(string userId)
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Retrieves all rejected approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of rejected approval requests.</returns>
    public async Task<IEnumerable<T>> GetMyRejectedRequestsAsync(string userId)
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Retrieves an approval request by its ID.
    /// </summary>
    /// <param name="id">The approval request ID.</param>
    /// <returns>The approval request entity, or null if not found.</returns>
    public async Task<ApprovalRequest?> GetRequestByIdAsync(Guid id)
    {   
        return await _context.ApprovalRequests
            .Include(p => p.Steps)
            .Include(p => p.Attachments)
            .SingleOrDefaultAsync(request => request.ApprovalId == id);
    }

    /// <summary>
    /// Approves an approval request by its ID.
    /// </summary>
    /// <param name="id">The approval request ID.</param>
    public async Task ApproveRequestAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            // Update entity to mark it as approved
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Rejects an approval request by its ID.
    /// </summary>
    /// <param name="id">The approval request ID.</param>
    public async Task RejectRequestAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            // Update entity to mark it as rejected
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    /// <returns>The entity, or null if not found.</returns>
    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The entity ID.</param>
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
