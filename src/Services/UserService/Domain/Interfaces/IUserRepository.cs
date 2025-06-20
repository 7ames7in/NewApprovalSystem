using UserService.Domain.Entities;
using BuildingBlocks.Core.Interfaces;

namespace UserService.Domain.Interfaces;

public interface IUserRepository<T> : IRepository<T> where T : class
{
    Task<T?> GetByEmployeeNoAsync(string employeeNumber);
    Task<IEnumerable<T>> GetUsersByRoleAsync(string roleName);
    Task<IAsyncEnumerable<T>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize);
    Task<bool> UserExistsAsync(string email);
    Task AddUserAsync(T user);
    Task UpdateUserAsync(T user);
    Task DeleteUserAsync(Guid userId);
    // LoginAttempt
    Task<T?> ValidateLoginAndInfomationAsync(string emailId);    
}
