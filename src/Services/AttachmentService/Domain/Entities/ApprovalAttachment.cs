public class ApprovalAttachment
{
    public Guid AttachmentId { get; set; }
    public Guid ApprovalId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
}
