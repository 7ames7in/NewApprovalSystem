using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserRepository<T> : IUserRepository<T> where T : class
{
    private readonly UserDbContext _context;

    public async Task<T?> ValidateLoginAndInfomationAsync(string email)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e =>
            EF.Property<string>(e, "Email") == email);
    }

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetUsersByRoleAsync(string role)
    {
        return await _context.Set<T>().Where(e => EF.Property<string>(e, "Role") == role).ToListAsync();
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Set<T>().AnyAsync(e => EF.Property<string>(e, "Email") == email);
    }

    public async Task AddUserAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
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
    // public async Task<IAsyncEnumerable<T>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize)
    // {
    //     // return await _context.Set<T>()
    //     //     .Where(e => EF.Functions.Like(EF.Property<string>(e, "Name"), $"%{searchTerm}%") ||
    //     //                 EF.Functions.Like(EF.Property<string>(e, "Email"), $"%{searchTerm}%"))
    //     //     .Skip((pageNumber - 1) * pageSize)
    //     //     .Take(pageSize)
    //     //     .ToListAsync();
    // }

    public Task<IAsyncEnumerable<T>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        var query = _context.Set<T>()
            .Where(e => EF.Functions.Like(EF.Property<string>(e, "Name"), $"%{searchTerm}%") ||
                        EF.Functions.Like(EF.Property<string>(e, "Email"), $"%{searchTerm}%"))
            .Skip(skip)
            .Take(pageSize)
            .AsAsyncEnumerable();

        return Task.FromResult(query);
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
    public async Task<T?> GetByEmailAsync(string email)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Email") == email);
    }
    public async Task<IEnumerable<T>> GetByRoleAsync(string role)
    {
        return await _context.Set<T>().Where(e => EF.Property<string>(e, "Role") == role).ToListAsync();
    }
}
