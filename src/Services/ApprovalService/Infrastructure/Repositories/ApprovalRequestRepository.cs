using Microsoft.EntityFrameworkCore;

public class ApprovalRequestRepository : IApprovalRequestRepository
{
    private readonly DbContext _context;

    public ApprovalRequestRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<ApprovalRequest?> GetByIdAsync(Guid id) =>
        await _context.Set<ApprovalRequest>().FindAsync(id);

    public async Task<IEnumerable<ApprovalRequest>> GetAllAsync() =>
        await _context.Set<ApprovalRequest>().ToListAsync();

    public async Task AddAsync(ApprovalRequest entity)
    {
        await _context.Set<ApprovalRequest>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApprovalRequest entity)
    {
        _context.Set<ApprovalRequest>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<ApprovalRequest>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<ApprovalRequest>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
