namespace ApprovalPaging.Models;

public class ApprovalAttachmentViewModel
{
    /// <summary>
    /// Unique identifier for the attachment.
    /// </summary>
    public Guid AttachmentId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier for the associated approval.
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
    /// Size of the file in bytes. Nullable.
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// Type of the file (e.g., MIME type). Nullable.
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
}

