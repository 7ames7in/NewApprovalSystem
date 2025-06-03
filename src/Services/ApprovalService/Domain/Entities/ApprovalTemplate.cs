using System;
using System.ComponentModel.DataAnnotations;

// Represents an approval template entity in the domain
public class ApprovalTemplate
{
    // Primary key for the approval template
    [Key]
    public Guid TemplateId { get; set; }

    // Name of the approval template
    public string TemplateName { get; set; }

    // Content of the approval template (optional)
    public string? TemplateContent { get; set; }

    // Employee number of the creator
    public string CreatedByEmployeeNumber { get; set; }

    // Timestamp when the template was created
    public DateTime CreatedAt { get; set; }
}
