namespace ApprovalService.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

public class ApprovalRequest
{
    [Key] // Entity Framework recognizes this as the primary key
    [Required] // This field is required
    [Display(Name = "Approval ID")] // Display name for UI purposes
    [Description("Unique identifier for the approval request.")] // Description for documentation
    public Guid ApprovalId { get; set; } // Unique identifier for the approval request
    public string RequestTitle { get; set; } = string.Empty; // Title of the approval request
    public string? RequestContent { get; set; } // Detailed content of the request
    public string ApplicantEmployeeNumber { get; set; } = string.Empty; // Employee number of the applicant
    public string ApplicantName { get; set; } = string.Empty; // Name of the applicant
    public string? ApplicantPosition { get; set; } // Position of the applicant
    public string? ApplicantDepartment { get; set; } // Department of the applicant
    public string Status { get; set; } = "Pending"; // Current status of the request (e.g., Pending, Approved, Rejected)
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow; // Timestamp when the request was created
    public DateTime? RespondedAt { get; set; } // Timestamp when the request was responded to
    public string? ApproverComment { get; set; } // Comments from the approver
    public string? ApprovalType { get; set; } // Type/category of the approval request
    public int CurrentStep { get; set; } = 1; // Current step in the approval process
    public string? CurrentApproverEmployeeNumber { get; set; } // Employee number of the current approver
    public string? MisKey { get; set; } // Optional key for integration with external systems
    public List<ApprovalStep> Steps { get; set; } = new List<ApprovalStep>(); // List of steps in the approval process
    public List<ApprovalAttachment> Attachments { get; set; } = new List<ApprovalAttachment>(); // List of attachments related to the request
}
