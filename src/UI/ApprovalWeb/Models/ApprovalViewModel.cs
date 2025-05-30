namespace ApprovalWeb.Models;
public class ApprovalViewModel
{
    public string RequestId { get; set; } = string.Empty;
    public string ApproverName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Comments { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }

    public string Priority { get; set; } = "Normal";

    public string Title { get; set; } = "Approval Request";
    public string Description { get; set; } = "Please review the approval request details below.";
    public string Details { get; set; } = string.Empty;
}