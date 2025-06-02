using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using ApprovalWeb.Services;

public class MockApprovalService : IApprovalService
{
    public Task<List<ApprovalRequestViewModel>> GetApprovalRequestsAsync()
        => Task.FromResult(MockApprovalData.Approvals);

    public Task<List<ApprovalRequestViewModel>> GetMyApprovalRequestsAsync(string userId)
        => Task.FromResult(MockApprovalData.Approvals);

    public Task<Guid> SubmitApprovalAsync(ApprovalRequestViewModel model)
    {
        MockApprovalData.Add(model);
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<ApprovalRequestViewModel?> GetByIdAsync(string requestId)
        => Task.FromResult(MockApprovalData.GetByRequestId(requestId));

    public Task UpdateStatusAsync(string requestId, string status, string? comment)
    {
        MockApprovalData.UpdateStatus(requestId, status, comment);
        return Task.CompletedTask;
    }
}
