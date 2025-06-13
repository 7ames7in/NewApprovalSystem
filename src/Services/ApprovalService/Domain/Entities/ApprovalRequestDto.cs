namespace ApprovalService.Domain.Entities;

public class ApprovalRequestWithCurrentStepDto : ApprovalRequest
{
    // 조인한 값
    public string? CurrentApproverName { get; set; }
    public string? CurrentActionStatus { get; set; }
}