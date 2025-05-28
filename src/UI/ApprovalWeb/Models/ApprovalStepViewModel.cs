namespace ApprovalWeb.Models;

public class ApprovalStepViewModel
{
    public Guid StepId { get; set; }
    public Guid ApprovalId { get; set; }
    public string ApproverEmployeeNumber { get; set; } = default!;
    public string ApproverName { get; set; } = default!;
    public int Sequence { get; set; } = 1;
    public bool IsFinalApprover { get; set; }
    public string? ActionStatus { get; set; }
    public string StepType { get; set; } = default!;
    public DateTime? ActionDate { get; set; }
    public string? Comment { get; set; }
}
