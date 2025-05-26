public interface IApprovalAttachmentRepository
{
    Task<ApprovalAttachment?> GetByIdAsync(Guid id);
    Task<IEnumerable<ApprovalAttachment>> GetAllAsync();
    Task AddAsync(ApprovalAttachment entity);
    Task UpdateAsync(ApprovalAttachment entity);
    Task DeleteAsync(Guid id);
}
