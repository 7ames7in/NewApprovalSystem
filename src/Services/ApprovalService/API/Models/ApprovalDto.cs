namespace ApprovalService.API.Models;

// DTO for transferring approval information between API and client
public class ApprovalDto
{
    public Guid ApprovalId { get; set; } // Unique identifier for the approval
    public string RequestId { get; set; } = string.Empty; // Identifier for the request
    public string ApproverName { get; set; } = string.Empty; // Name of the approver
    public string Status { get; set; } = string.Empty; // Current status of the approval
    public string? Comments { get; set; } // Optional comments from the approver
    public DateTime RequestedAt { get; set; } // Timestamp when the approval was requested
}
