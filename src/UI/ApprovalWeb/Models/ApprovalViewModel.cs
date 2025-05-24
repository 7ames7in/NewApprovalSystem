namespace ApprovalWeb.Models
{
    public class ApprovalViewModel
    {
        public string RequestId { get; set; } = string.Empty;
        public string ApproverName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Comments { get; set; }
    }
}
