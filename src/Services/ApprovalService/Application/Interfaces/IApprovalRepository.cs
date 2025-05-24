using ApprovalService.Domain.Entities;

namespace ApprovalService.Application.Interfaces;

public interface IApprovalRepository
{
    Task AddAsync(Approval approval);
    Task<Approval?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
