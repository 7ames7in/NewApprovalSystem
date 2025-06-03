namespace ApprovalService.API.Models;

// DTO for transferring approval request information between API and client
public class ApprovalRequestDto
{
    public Guid ApprovalId { get; set; } // Unique identifier for the approval
    public string RequestTitle { get; set; } = string.Empty; // Title of the request
    public string? RequestContent { get; set; } // Optional content/details of the request
    public string ApplicantEmployeeNumber { get; set; } = string.Empty; // Employee number of the applicant
    public string ApplicantName { get; set; } = string.Empty; // Name of the applicant
    public string? ApplicantPosition { get; set; } // Optional position of the applicant
    public string? ApplicantDepartment { get; set; } // Optional department of the applicant
    public string Status { get; set; } = "Pending"; // Current status of the request (default: Pending)
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow; // Timestamp when the request was created
    public DateTime? RespondedAt { get; set; } // Optional timestamp when the request was responded to
    public string? ApproverComment { get; set; } // Optional comment from the approver
    public string? ApprovalType { get; set; } // Optional type/category of the approval
    public string? MisKey { get; set; } // Optional key for MIS (Management Information System)
}
