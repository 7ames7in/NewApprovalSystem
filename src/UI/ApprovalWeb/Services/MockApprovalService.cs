using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using ApprovalWeb.Services;

public class MockApprovalService : IApprovalService
{
    public Task<List<ApprovalViewModel>> GetApprovalsAsync()
        => Task.FromResult(MockApprovalData.Approvals);

    public Task<Guid> SubmitApprovalAsync(ApprovalViewModel model)
    {
        MockApprovalData.Add(model);
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<ApprovalViewModel?> GetByIdAsync(string requestId)
        => Task.FromResult(MockApprovalData.GetByRequestId(requestId));

    public Task UpdateStatusAsync(string requestId, string status, string? comment)
    {
        MockApprovalData.UpdateStatus(requestId, status, comment);
        return Task.CompletedTask;
    }
}
