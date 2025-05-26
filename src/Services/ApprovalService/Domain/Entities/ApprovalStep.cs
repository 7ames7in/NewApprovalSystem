public class ApprovalStep
{
    public Guid StepId { get; set; }
    public Guid ApprovalId { get; set; }
    public string ApproverEmployeeNumber { get; set; }
    public int Sequence { get; set; } = 1;
    public bool IsFinalApprover { get; set; }
    public string? ActionStatus { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? Comment { get; set; }
}
