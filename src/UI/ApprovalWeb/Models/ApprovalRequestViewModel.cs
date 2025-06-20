using BuildingBlocks.Core.Pagination;

namespace ApprovalWeb.Models;

/// <summary>
/// ViewModel representing an approval request.
/// </summary>
public class ApprovalRequestViewModel
{
    /// <summary>
    /// Unique identifier for the approval request.
    /// </summary>
    public Guid? ApprovalId { get; set; }

    /// <summary>
    /// Title of the approval request.
    /// </summary>
    public string RequestTitle { get; set; } = default!;

    /// <summary>
    /// Content or description of the approval request.
    /// </summary>
    public string? RequestContent { get; set; }

    /// <summary>
    /// Employee number of the applicant.
    /// </summary>
    public string ApplicantEmployeeNumber { get; set; } = default!;

    /// <summary>
    /// Name of the applicant.
    /// </summary>
    public string ApplicantName { get; set; } = default!;

    /// <summary>
    /// Position of the applicant.
    /// </summary>
    public string? ApplicantPosition { get; set; }

    /// <summary>
    /// Department of the applicant.
    /// </summary>
    public string? ApplicantDepartment { get; set; }

    /// <summary>
    /// Current status of the approval request (default is "Pending").
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Date and time when the request was created.
    /// </summary>
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the request was responded to.
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// Comment provided by the approver.
    /// </summary>
    public string? ApproverComment { get; set; }

    /// <summary>
    /// Type of approval (e.g., financial, administrative).
    /// </summary>
    public string? ApprovalType { get; set; }

    /// <summary>
    /// Key for integration with MIS (Management Information System).
    /// </summary>
    public string? MisKey { get; set; }

    /// <summary>
    /// JSON representation of the approval steps.
    /// </summary>
    public string? StepsJson { get; set; } = string.Empty;
    public string? CurrentApproverName { get; set; } = string.Empty;
    public string? CurrentApproverEmployeeNumber { get; set; } = string.Empty;
    public string? CurrentActionStatus { get; set; } = "Pending";

    /// <summary>
    /// List of approval steps associated with the request.
    /// </summary>
    public List<ApprovalStepViewModel> Steps { get; set; } = new List<ApprovalStepViewModel>();

    /// <summary>
    /// List of attachments associated with the request.
    /// </summary>
    public List<ApprovalAttachmentViewModel> Attachments { get; set; } = new List<ApprovalAttachmentViewModel>();

    /// <summary>
    /// List of files uploaded by the applicant.
    /// </summary>
    public List<IFormFile>? Files { get; set; }  // ✅ Must be public and match the property name "files".
    public static IComparer<ApprovalRequestViewModel>? DateComparer => Comparer<ApprovalRequestViewModel>.Create((x, y) => y.RequestedAt.CompareTo(x.RequestedAt));
    public static IComparer<ApprovalRequestViewModel>? TitleComparer => Comparer<ApprovalRequestViewModel>.Create((x, y) => string.Compare(x.RequestTitle, y.RequestTitle, StringComparison.OrdinalIgnoreCase));
}
