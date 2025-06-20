using ApprovalWeb.Models;

namespace ApprovalWeb.Interfaces;

public interface IAttachmentService
{
     Task<IEnumerable<ApprovalAttachmentViewModel>> GetAttachmentsAsync(string requestId);
     Task<ApprovalAttachmentViewModel?> GetAttachmentByIdAsync(string attachmentId);
     Task<bool> AddAttachmentAsync(string requestId, ApprovalAttachmentViewModel attachment);
     Task<bool> DeleteAttachmentAsync(string attachmentId);
     Task<bool> UpdateAttachmentAsync(ApprovalAttachmentViewModel attachment);
}