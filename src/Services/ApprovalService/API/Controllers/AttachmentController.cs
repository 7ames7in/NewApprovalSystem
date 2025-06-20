using System.Net.Mail;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApprovalService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentRepository<ApprovalAttachment> _attachmentService;
        public AttachmentController(IAttachmentRepository<ApprovalAttachment> attachmentService)
        {
            _attachmentService = attachmentService ?? throw new ArgumentNullException(nameof(attachmentService));
        }

        /// <summary>
        /// Downloads an attachment by its ID.
        /// </summary>
        /// <param name="id">The ID of the attachment.</param>
        /// <returns>The file content as a downloadable file.</returns>
        [HttpGet("{attachmentId}")]
        public async Task<IActionResult> DownloadAttachment(string attachmentId)
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(attachmentId);
            if (attachment == null)
                return NotFound();

            return Ok(attachment);
        }
    }
}
