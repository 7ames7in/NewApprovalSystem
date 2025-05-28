using UserService.Domain.Entities;
using BuildingBlocks.Core.Infrastructure.Data.Interfaces;

namespace UserService.Domain.Interfaces;

public interface IUserRepository<T> : IRepository<T> where T : class
{
    Task<T?> GetByEmailAsync(string email);
    Task<IEnumerable<T>> GetUsersByRoleAsync(string roleName);
    Task<IAsyncEnumerable<T>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize);
    Task<bool> UserExistsAsync(string email);
    Task AddUserAsync(T user);
    Task UpdateUserAsync(T user);
    Task DeleteUserAsync(Guid userId);
}
