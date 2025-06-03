using BuildingBlocks.Core.Infrastructure.Data.Interfaces;

namespace ApprovalService.Domain.Interfaces;

/// <summary>
/// Interface for Approval Repository.
/// Provides methods to interact with approval requests.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IApprovalRepository<T> : IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves the rejected approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the rejected approval requests.</returns>
    Task<IEnumerable<T>> GetMyRejectedApprovalRequestsAsync(string userId);

    /// <summary>
    /// Retrieves all approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the approval requests.</returns>
    Task<IEnumerable<T>> GetMyApprovalRequestsAsync(string userId);

    /// <summary>
    /// Retrieves the pending approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the pending approval requests.</returns>
    Task<IEnumerable<T>> GetMyPendingApprovalRequestsAsync(string userId);

    /// <summary>
    /// Retrieves the approved approval requests for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the approved approval requests.</returns>
    Task<IEnumerable<T>> GetMyApprovedApprovalRequestsAsync(string userId);
}