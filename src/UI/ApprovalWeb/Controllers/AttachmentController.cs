using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApprovalWeb.Interfaces;

namespace ApprovalWeb.Controllers
{
    [Authorize]
    public class AttachmentController : BaseController
    {
        private readonly IAttachmentService _attachmentService;
        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService ?? throw new ArgumentNullException(nameof(attachmentService));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAttachment(string id)
        {
            // 첨부파일 조회 및 파일 반환 로직
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(attachment.FilePath);
            return File(fileBytes, "application/octet-stream", attachment.FileName);
        }
    }

}
