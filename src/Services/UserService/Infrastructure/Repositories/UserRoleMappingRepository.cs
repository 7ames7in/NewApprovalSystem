public class UserRoleMappingRepository : IUserRoleMappingRepository
{
    private readonly DbContext _context;

    public UserRoleMappingRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<UserRoleMapping?> GetByIdAsync(Guid id) =>
        await _context.Set<UserRoleMapping>().FindAsync(id);

    public async Task<IEnumerable<UserRoleMapping>> GetAllAsync() =>
        await _context.Set<UserRoleMapping>().ToListAsync();

    public async Task AddAsync(UserRoleMapping entity)
    {
        await _context.Set<UserRoleMapping>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserRoleMapping entity)
    {
        _context.Set<UserRoleMapping>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<UserRoleMapping>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<UserRoleMapping>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
