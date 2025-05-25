using ApprovalWeb.Models;

namespace ApprovalWeb.Interfaces;

public interface IApprovalService
{
    Task<List<ApprovalViewModel>> GetApprovalsAsync();
    Task<Guid> SubmitApprovalAsync(ApprovalViewModel model);
    Task<ApprovalViewModel?> GetByIdAsync(string requestId);
    Task UpdateStatusAsync(string requestId, string status, string? comment);
}
