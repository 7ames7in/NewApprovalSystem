namespace ApprovalService.Domain.Entities;

public class ApprovalRequest
{
    public Guid ApprovalId { get; set; }
    public string RequestTitle { get; set; }
    public string? RequestContent { get; set; }
    public string ApplicantEmployeeNumber { get; set; }
    public string ApplicantName { get; set; }
    public string? ApplicantPosition { get; set; }
    public string? ApplicantDepartment { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime RequestedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? ApproverComment { get; set; }
    public string? ApprovalType { get; set; }
    public string? MisKey { get; set; }
}
