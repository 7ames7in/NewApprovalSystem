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
