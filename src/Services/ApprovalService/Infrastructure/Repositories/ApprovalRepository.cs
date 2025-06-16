using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using BuildingBlocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ApprovalService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for managing ApprovalRequest entities.
/// </summary>
/// <typeparam name="T">The type of ApprovalRequest.</typeparam>
public class ApprovalRepository<T> : IApprovalRepository<T> where T : ApprovalRequestWithCurrentStepDto
{
    private readonly ApprovalDbContext _context;
    private readonly ILogger<ApprovalRepository<T>> _logger;
    //private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Initializes a new instance of the ApprovalRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ApprovalRepository(ApprovalDbContext context, ILogger<ApprovalRepository<T>> logger /*, IEventPublisher eventPublisher*/)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //_eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
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
    public async Task<IEnumerable<ApprovalRequestWithCurrentStepDto>> GetMyApprovalRequestsAsync(string userId)
    {
        // return await _context.Set<T>()
        //     .Where(approval => approval.Steps.Any(step => step.ApproverEmployeeNumber == userId))
        //     .Include(approval => approval.Steps)
        //     .ToListAsync();

        return await _context.ApprovalRequests
            .Where(approval => approval.Steps.Any(step => step.ApproverEmployeeNumber == userId))
            .Include(approval => approval.Steps)
            .Select(r => new ApprovalRequestWithCurrentStepDto
            {
                ApprovalId = r.ApprovalId,
                RequestTitle = r.RequestTitle,
                RequestedAt = r.RequestedAt,
                RespondedAt = r.RespondedAt,
                ApplicantEmployeeNumber = r.ApplicantEmployeeNumber,
                ApplicantDepartment = r.ApplicantDepartment,
                ApplicantName = r.ApplicantName,
                ApplicantPosition = r.ApplicantPosition,
                Status = r.Status,
                Steps = r.Steps,
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
            }).ToListAsync();
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

    public async Task<bool> ApproveRequestAsync(string approvalId, string comment, string approverEmployeeNumber)
    {
        // 1. Guid 파싱 (DB 키가 Guid니까)
        if (!Guid.TryParse(approvalId, out var approvalGuid))
        {
            return false;
        }

        // 2. ApprovalRequest 로드 (Steps 포함하여 한번에 가져옴)
        var request = await _context.ApprovalRequests
            .Include(r => r.Steps)
            .FirstOrDefaultAsync(r => r.ApprovalId == approvalGuid);

        if (request == null)
            return false;

        // 3. Steps에서 현재 Approver 찾기
        var currentStep = request.Steps
            .FirstOrDefault(s => s.ApproverEmployeeNumber == approverEmployeeNumber && s.Sequence == request.CurrentStep);

        if (currentStep == null)
            return false;

        // 4. ApprovalStep 업데이트
        currentStep.ActionStatus = "Approved";
        currentStep.ActionDate = DateTime.UtcNow;
        currentStep.Comment = comment;

        // 5. ApprovalRequest 업데이트
        request.CurrentApproverEmployeeNumber = approverEmployeeNumber;
        request.RespondedAt = DateTime.UtcNow;

        // 6. 다음 단계 이동 or 최종 승인 처리
        if (request.CurrentStep >= request.Steps.Max(s => s.Sequence))
        {
            request.Status = "Approved";
        }
        else
        {
            request.CurrentStep++; // 다음 Step으로 이동
            request.CurrentApproverEmployeeNumber = request.Steps
                .FirstOrDefault(s => s.Sequence == request.CurrentStep)?.ApproverEmployeeNumber;
        }

        await _context.SaveChangesAsync();

        // 7. 성공적으로 승인 처리됨
        // 로깅 추가 가능
        _logger.LogInformation($"Approval {approvalId} approved by {approverEmployeeNumber} with comment: {comment}");
        // Email Send 이벤트 발행 추가 가능
        // 예: await _eventPublisher.PublishAsync(new ApprovalApprovedEvent(approvalId, approverEmployeeNumber, comment));
        // 8. 성공적으로 승인 처리됨
        // 로깅 추가 가능
        // 예: _logger.LogInformation($"Approval {approvalId} approved by {approverEmployeeNumber} with comment: {comment}");
        
        return true;
    }

    public Task<bool> RejectRequestAsync(string approveId, string? comment, string approverEmployeeNumber)
    {
        throw new NotImplementedException();
    }
}

