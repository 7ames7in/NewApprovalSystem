namespace BuildingBlocks.EventContracts;

public class EmailEvent
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<AttachmentDto>? Attachments { get; set; }
}
