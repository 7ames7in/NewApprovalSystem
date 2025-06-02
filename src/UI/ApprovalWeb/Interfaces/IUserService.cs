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
    //LoginAttempt
    //Task<bool> ValidateLoginAsync(string emailid);
    Task<T> UserLoginAndInformationAsync(string emailid);
    Task<T?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<T>> GetUsersByRoleAsync(string role);
}