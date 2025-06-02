using ApprovalService.Domain.Entities;
using System.Threading.Tasks;

namespace ApprovalService.Application.Interfaces;

public interface IApprovalRepository
{
    Task AddAsync(ApprovalRequest approval);
    Task SaveChangesAsync();
}
