using ApprovalWeb.Models;

namespace ApprovalWeb.Interfaces;

public interface IApprovalService
{
    Task<IEnumerable<ApprovalRequestViewModel>> GetMyApprovalRequestsAsync(string approverId);
    Task<Guid> SubmitApprovalAsync(ApprovalRequestViewModel model);

    Task<bool> ApproveRequestAsync(string approverId, string? comment, string approverEmployeeNumber);
    Task<bool> RejectRequestAsync(string approverId, string? comment, string approverEmployeeNumber);
    Task<ApprovalRequestViewModel?> GetByIdAsync(string requestId);
    Task UpdateStatusAsync(string requestId, string status, string? comment);
}
