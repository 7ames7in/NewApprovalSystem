using ApprovalService.Domain.Entities;
using System.Threading.Tasks;

namespace ApprovalService.Application.Interfaces;

public interface IApprovalRepository
{
    Task AddAsync(Approval approval);
    Task<Approval?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
