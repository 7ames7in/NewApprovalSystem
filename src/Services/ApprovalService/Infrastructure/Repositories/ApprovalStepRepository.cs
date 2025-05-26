using Microsoft.EntityFrameworkCore;

public class ApprovalStepRepository : IApprovalStepRepository
{
    private readonly DbContext _context;

    public ApprovalStepRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<ApprovalStep?> GetByIdAsync(Guid id) =>
        await _context.Set<ApprovalStep>().FindAsync(id);

    public async Task<IEnumerable<ApprovalStep>> GetAllAsync() =>
        await _context.Set<ApprovalStep>().ToListAsync();

    public async Task AddAsync(ApprovalStep entity)
    {
        await _context.Set<ApprovalStep>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApprovalStep entity)
    {
        _context.Set<ApprovalStep>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<ApprovalStep>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<ApprovalStep>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
