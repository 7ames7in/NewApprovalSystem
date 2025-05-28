using ApprovalWeb.Models;

namespace ApprovalWeb.Interfaces;

public interface IUserService<T> where T : class
{
    Task<IEnumerable<T>> GetAllUsersAsync();
    Task<T> GetUserByIdAsync(int id);
    Task<IEnumerable<T>> SearchUsersAsync(string query);
    Task<T> CreateUserAsync(T user);
    Task<bool> UpdateUserAsync(int id, T updatedUser);
    Task<bool> DeleteUserAsync(int id);
}