using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;


public class ApprovalStepRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;

    public ApprovalStepRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<T?> FindByAsync(string value)
    {
        // Implement a method to find an entity by a specific string value.
        // This is a placeholder implementation; you should customize it based on your entity's properties.
        return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == value);
    }
}
