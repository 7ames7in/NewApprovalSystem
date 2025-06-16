namespace ApprovalPaging.Models;

public class ApprovalStepViewModel
{
    public Guid StepId { get; set; }
    public Guid ApprovalId { get; set; }
    public string ApproverEmployeeNumber { get; set; } = default!;
    public string ApproverName { get; set; } = default!;
    public string? Department { get; set; } = string.Empty;
    public string? Position { get; set; } = string.Empty;
    public string? JobTitle { get; set; } = string.Empty;
    public int Sequence { get; set; } = 1;
    public bool IsFinalApprover { get; set; } = true;
    public string? ActionStatus { get; set; } = "Pending";
    public string StepType { get; set; } = default!;
    public DateTime? ActionDate { get; set; }
    public string? Comment { get; set; }
    
}
