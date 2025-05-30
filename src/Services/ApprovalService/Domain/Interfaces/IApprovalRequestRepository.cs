using BuildingBlocks.Core.Infrastructure.Data.Interfaces;

namespace ApprovalService.Domain.Interfaces;

public interface IApprovalRequestRepository<T> : IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetMyRequestsAsync(string userId);
    Task<IEnumerable<T>> GetMyPendingRequestsAsync(string userId);
    Task<IEnumerable<T>> GetMyApprovedRequestsAsync(string userId);
    Task<IEnumerable<T>> GetMyRejectedRequestsAsync(string userId);
    Task<T?> GetRequestByIdAsync(Guid requestId);
    Task ApproveRequestAsync(Guid requestId);
    Task RejectRequestAsync(Guid requestId);
    Task<T> CreateApprovalRequestAsync(T request);
}
