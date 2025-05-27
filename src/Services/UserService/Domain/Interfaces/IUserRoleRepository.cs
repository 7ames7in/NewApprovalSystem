using UserService.Domain.Entities;

//하지만 지금은 사용하지 않음. 공통으로 뺐음.

public interface IUserRoleRepository
{
    Task<UserRole?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserRole>> GetAllAsync();
    Task AddAsync(UserRole entity);
    Task UpdateAsync(UserRole entity);
    Task DeleteAsync(Guid id);
}
