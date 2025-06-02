using BuildingBlocks.Core.Infrastructure.Data.Interfaces;

namespace ApprovalService.Domain.Interfaces;

public interface IApprovalRepository<T> : IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetMyRejectedApprovalRequestsAsync(string userId);
    Task<IEnumerable<T>> GetMyApprovalRequestsAsync(string userId);
    Task<IEnumerable<T>> GetMyPendingApprovalRequestsAsync(string userId);
    Task<IEnumerable<T>> GetMyApprovedApprovalRequestsAsync(string userId);

}