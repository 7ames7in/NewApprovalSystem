using Microsoft.EntityFrameworkCore;

public class ApprovalTemplateRepository : IApprovalTemplateRepository
{
    private readonly DbContext _context;

    public ApprovalTemplateRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<ApprovalTemplate?> GetByIdAsync(Guid id) =>
        await _context.Set<ApprovalTemplate>().FindAsync(id);

    public async Task<IEnumerable<ApprovalTemplate>> GetAllAsync() =>
        await _context.Set<ApprovalTemplate>().ToListAsync();

    public async Task AddAsync(ApprovalTemplate entity)
    {
        await _context.Set<ApprovalTemplate>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApprovalTemplate entity)
    {
        _context.Set<ApprovalTemplate>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<ApprovalTemplate>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<ApprovalTemplate>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
