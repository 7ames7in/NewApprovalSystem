namespace ApprovalWeb.Models;

public class ApprovalRequestViewModel
{
    public Guid? ApprovalId { get; set; }
    public string RequestTitle { get; set; } = default!;
    public string? RequestContent { get; set; }
    public string ApplicantEmployeeNumber { get; set; } = default!;
    public string ApplicantName { get; set; } = default!;
    public string? ApplicantPosition { get; set; }
    public string? ApplicantDepartment { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }
    public string? ApproverComment { get; set; }
    public string? ApprovalType { get; set; }
    public string? MisKey { get; set; }
    public string? StepsJson { get; set; } = string.Empty;

    public List<ApprovalStepViewModel> Steps { get; set; } = new List<ApprovalStepViewModel>();
    public List<ApprovalAttachmentViewModel> Attachments { get; set; } = new List<ApprovalAttachmentViewModel>();
}
