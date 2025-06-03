using System.ComponentModel.DataAnnotations;

namespace ApprovalService.Domain.Entities;

/// <summary>
/// Represents a step in the approval process.
/// </summary>
public class ApprovalStep
{
    /// <summary>
    /// Unique identifier for the approval step.
    /// </summary>
    [Key]
    public Guid StepId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier for the associated approval request.
    /// </summary>
    public Guid ApprovalId { get; set; }

    /// <summary>
    /// Employee number of the approver.
    /// </summary>
    public string ApproverEmployeeNumber { get; set; }

    /// <summary>
    /// Sequence number of the step in the approval process.
    /// </summary>
    public int Sequence { get; set; } = 1;

    /// <summary>
    /// Indicates whether this step is the final approver step.
    /// </summary>
    public bool IsFinalApprover { get; set; } = true;

    /// <summary>
    /// Status of the action taken on this step (e.g., "Approved", "Rejected").
    /// </summary>
    public string? ActionStatus { get; set; }

    /// <summary>
    /// Date when the action was taken on this step.
    /// </summary>
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// Comment or note provided during the action.
    /// </summary>
    public string? Comment { get; set; }

    // Additional properties for managing details

    /// <summary>
    /// Name of the approver.
    /// </summary>
    public string? ApproverName { get; set; } = string.Empty;

    /// <summary>
    /// Position of the approver within the organization.
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// Department of the approver.
    /// </summary>
    public string? Department { get; set; } = string.Empty;

    /// <summary>
    /// Type of the step (e.g., "Agreement", "Review", "Approval").
    /// </summary>
    public string? StepType { get; set; } = string.Empty;

    // Relationship properties

    /// <summary>
    /// Associated approval request entity.
    /// </summary>
    public ApprovalRequest? ApprovalRequest { get; set; } = default!;
}
