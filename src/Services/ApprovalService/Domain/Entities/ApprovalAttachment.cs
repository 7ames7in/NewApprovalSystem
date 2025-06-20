using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApprovalService.Domain.Entities;

/// <summary>
/// Represents an attachment associated with an approval request.
/// </summary>
public class ApprovalAttachment
{
    /// <summary>
    /// Unique identifier for the attachment.
    /// </summary>
    [Key]
    [Required]
    [Display(Name = "Attachment ID")]
    public Guid AttachmentId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the associated approval request.
    /// </summary>
    public Guid ApprovalId { get; set; }

    /// <summary>
    /// Name of the file.
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Path where the file is stored.
    /// </summary>
    public string FilePath { get; set; } = default!;

    /// <summary>
    /// Size of the file in bytes. Nullable if size is unknown.
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// Type of the file (e.g., MIME type). Nullable if type is unknown.
    /// </summary>
    public string? FileType { get; set; } = default!;

    /// <summary>
    /// Date and time when the file was uploaded.
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    /// Employee number of the person who uploaded the file.
    /// </summary>
    public string UploadedByEmployeeNumber { get; set; } = default!;

    /// <summary>
    /// Navigation property to the associated approval request.
    /// </summary>
    public ApprovalRequest? ApprovalRequest { get; set; } = default!;
}