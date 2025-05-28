using ApprovalWeb.Models;

namespace ApprovalWeb.Interfaces;

public interface IApprovalRequestService
{
    Task<IEnumerable<ApprovalRequestViewModel>> GetMyRequestsAsync(string userId);
    Task<IEnumerable<ApprovalRequestViewModel>> GetMyPendingRequestsAsync(string userId);
    Task<IEnumerable<ApprovalRequestViewModel>> GetMyApprovedRequestsAsync(string userId);
    Task<IEnumerable<ApprovalRequestViewModel>> GetMyRejectedRequestsAsync(string userId);
    Task<ApprovalRequestViewModel?> GetRequestByIdAsync(string requestId);
    Task ApproveRequestAsync(string requestId);
    Task RejectRequestAsync(string requestId);
}
