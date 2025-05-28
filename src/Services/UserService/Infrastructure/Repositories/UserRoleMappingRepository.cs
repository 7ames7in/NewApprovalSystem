using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;
using UserService.Infrastructure.Persistence;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories;

public class UserRoleMappingRepository<T> : IUserRoleMappingRepository<T> where T : class
{
    private readonly UserDbContext _context;

    public UserRoleMappingRepository(UserDbContext context)
    {
        _context = context;
    }
    // Implementing IUserRoleMappingRepository methods
    public async Task<IEnumerable<T>> GetRolesByUserIdAsync(string userId)
    {
        return await _context.Set<T>().Where(r => EF.Property<string>(r, "UserId") == userId).ToListAsync();
    }
    public async Task<T?> GetRoleByIdAsync(Guid roleId)
    {
        return await _context.Set<T>().FindAsync(roleId);
    }
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // Implementing IRepository<T> methods
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
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
