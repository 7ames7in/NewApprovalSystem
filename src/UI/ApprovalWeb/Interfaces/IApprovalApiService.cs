using ApprovalWeb.Models;

namespace ApprovalWeb.Interfaces;

public interface IApprovalService
{
    Task<List<ApprovalRequestViewModel>> GetMyApprovalRequestsAsync(string approverId);
    Task<Guid> SubmitApprovalAsync(ApprovalRequestViewModel model);
    Task<ApprovalRequestViewModel?> GetByIdAsync(string requestId);
    Task UpdateStatusAsync(string requestId, string status, string? comment);
}
