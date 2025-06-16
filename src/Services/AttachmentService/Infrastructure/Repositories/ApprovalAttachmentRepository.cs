using BuildingBlocks.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using AttachmentService.Infrastructure.Persistence;

namespace AttachmentService.Infrastructure.Repositories;

public class ApprovalAttachmentRepository<T> : IRepository<T> where T : class
{
    private readonly AttachmentDbContext _context;

    public ApprovalAttachmentRepository(AttachmentDbContext context)
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
