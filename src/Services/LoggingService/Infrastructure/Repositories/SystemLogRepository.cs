using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Interfaces;
using LoggingService.Infrastructure.Persistence;

namespace LoggingService.Infrastructure.Repositories;

public class SystemLogRepository<T> : IRepository<T> where T : class
{
    private readonly SystemLogDbContext _context;

    public SystemLogRepository(SystemLogDbContext context)
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
}
