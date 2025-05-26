public interface IApprovalTemplateRepository
{
    Task<ApprovalTemplate?> GetByIdAsync(Guid id);
    Task<IEnumerable<ApprovalTemplate>> GetAllAsync();
    Task AddAsync(ApprovalTemplate entity);
    Task UpdateAsync(ApprovalTemplate entity);
    Task DeleteAsync(Guid id);
}
