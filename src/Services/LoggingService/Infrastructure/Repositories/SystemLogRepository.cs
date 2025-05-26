public class SystemLogRepository : ISystemLogRepository
{
    private readonly DbContext _context;

    public SystemLogRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<SystemLog?> GetByIdAsync(Guid id) =>
        await _context.Set<SystemLog>().FindAsync(id);

    public async Task<IEnumerable<SystemLog>> GetAllAsync() =>
        await _context.Set<SystemLog>().ToListAsync();

    public async Task AddAsync(SystemLog entity)
    {
        await _context.Set<SystemLog>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SystemLog entity)
    {
        _context.Set<SystemLog>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<SystemLog>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<SystemLog>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
