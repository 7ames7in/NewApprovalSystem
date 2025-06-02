namespace ApprovalService.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class ApprovalRequest
{
    [Key] // 또는 이름이 Id 혹은 ApprovalId이면 EF가 자동으로 인식함
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

    public List<ApprovalStep> Steps { get; set; } = new List<ApprovalStep>();

    public List<ApprovalAttachment> Attachments { get; set; } = new List<ApprovalAttachment>();
}
