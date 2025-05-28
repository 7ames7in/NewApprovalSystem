namespace ApprovalService.API.Models;

public class ApprovalDto
{
    public Guid ApprovalId { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public string ApproverName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Comments { get; set; }
    public DateTime RequestedAt { get; set; }
}

//ApprovalDto는 ApprovalRequest와 Approval 엔티티의 정보를 포함하는 DTO(Data Transfer Object)입니다.
// 이 DTO는 API에서 ApprovalRequest와 Approval 엔티티의 정보를 클라이언트에 전달하기 위해 사용됩니다.

// ApprovalRequestDto는 ApprovalRequest 엔티티의 정보를 포함합니다.
public class ApprovalRequestDto
{
    public Guid ApprovalId { get; set; }
    public string RequestTitle { get; set; } = string.Empty;
    public string? RequestContent { get; set; }
    public string ApplicantEmployeeNumber { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string? ApplicantPosition { get; set; }
    public string? ApplicantDepartment { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }
    public string? ApproverComment { get; set; }
    public string? ApprovalType { get; set; }
    public string? MisKey { get; set; }
}
// ApprovalRequestDto는 ApprovalRequest 엔티티의 정보를 포함하는 DTO(Data Transfer Object)입니다.