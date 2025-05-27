using UserService.Domain.Entities;

//하지만 지금은 사용하지 않음. 공통으로 뺐음.

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task DeleteAsync(Guid id);
}
