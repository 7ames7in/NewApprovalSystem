namespace ApprovalWeb.Models
{
    /// <summary>
    /// Represents the model for error views.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request ID associated with the error.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indicates whether the request ID should be displayed.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
