namespace ApprovalService.Domain.Entities;

public class ApprovalAttachment
{
    public Guid AttachmentId { get; set; }
    public Guid ApprovalId { get; set; }
    public string FileName { get; set; } = default!;
    public string FilePath { get; set; } = default!;
    public DateTime UploadedAt { get; set; }
    public string UploadedByEmployeeNumber { get; set; } = default!;
}