public interface IApprovalRequestRepository
{
    Task<ApprovalRequest?> GetByIdAsync(Guid id);
    Task<IEnumerable<ApprovalRequest>> GetAllAsync();
    Task AddAsync(ApprovalRequest entity);
    Task UpdateAsync(ApprovalRequest entity);
    Task DeleteAsync(Guid id);
}
