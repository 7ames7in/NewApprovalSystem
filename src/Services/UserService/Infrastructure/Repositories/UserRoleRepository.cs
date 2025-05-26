public class UserRoleRepository : IUserRoleRepository
{
    private readonly DbContext _context;

    public UserRoleRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<UserRole?> GetByIdAsync(Guid id) =>
        await _context.Set<UserRole>().FindAsync(id);

    public async Task<IEnumerable<UserRole>> GetAllAsync() =>
        await _context.Set<UserRole>().ToListAsync();

    public async Task AddAsync(UserRole entity)
    {
        await _context.Set<UserRole>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserRole entity)
    {
        _context.Set<UserRole>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<UserRole>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<UserRole>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
