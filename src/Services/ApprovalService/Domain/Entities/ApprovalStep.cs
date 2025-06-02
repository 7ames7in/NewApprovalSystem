using System.ComponentModel.DataAnnotations;

namespace ApprovalService.Domain.Entities;

public class ApprovalStep
{
    [Key]
    public Guid StepId { get; set; } = Guid.NewGuid();
    public Guid ApprovalId { get; set; }
    public string ApproverEmployeeNumber { get; set; }
    public int Sequence { get; set; } = 1;
    public bool IsFinalApprover { get; set; } = true;
    public string? ActionStatus { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? Comment { get; set; }

    // 내용을 추가로 관리하기 위한 속성들
    public string? ApproverName { get; set; } = string.Empty;
    public string? Position { get; set; } = string.Empty;

    // 🔽 관계 명확하게
    public ApprovalRequest? ApprovalRequest { get; set; } = default!;
}
