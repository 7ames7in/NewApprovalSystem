public class UserRepository : IUserRepository
{
    private readonly DbContext _context;

    public UserRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Set<User>().FindAsync(id);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _context.Set<User>().ToListAsync();

    public async Task AddAsync(User entity)
    {
        await _context.Set<User>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Set<User>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<User>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<User>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
