using ApprovalService.Domain.Entities;
using BuildingBlocks.Core.Interfaces;

namespace ApprovalService.Domain.Interfaces;

public interface IAttachmentRepository<T> : IRepository<T> where T : ApprovalAttachment
{
     /// <summary>
     ///     Deletes the attachment by its identifier.
     /// </summary>
     /// <param name="attachmentId">The identifier of the attachment to delete.</param>
     Task DeleteAttachment(string attachmentId);

     /// <summary>
     ///     Gets the attachment by its identifier.
     /// </summary>
     /// <param name="attachmentId">The identifier of the attachment to retrieve.</param>
     /// <returns>The attachment with the specified identifier.</returns>
     Task<T> GetAttachmentByIdAsync(string attachmentId);
}