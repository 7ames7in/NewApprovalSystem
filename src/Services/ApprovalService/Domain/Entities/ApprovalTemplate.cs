using System.ComponentModel.DataAnnotations;

public class ApprovalTemplate
{
    [Key]
    public Guid TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string? TemplateContent { get; set; }
    public string CreatedByEmployeeNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
