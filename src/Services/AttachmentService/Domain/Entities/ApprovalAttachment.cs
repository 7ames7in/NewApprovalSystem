using System.ComponentModel.DataAnnotations;

namespace AttachmentService.Domain.Entities;

public class ApprovalAttachment
{
    [Key]
    public Guid AttachmentId { get; set; }
    public Guid ApprovalId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
}
