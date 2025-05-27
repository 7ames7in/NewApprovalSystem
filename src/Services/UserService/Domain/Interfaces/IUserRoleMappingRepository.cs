using UserService.Domain.Entities;

//하지만 지금은 사용하지 않음. 공통으로 뺐음.

public interface IUserRoleMappingRepository
{
    Task<UserRoleMapping?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserRoleMapping>> GetAllAsync();
    Task AddAsync(UserRoleMapping entity);
    Task UpdateAsync(UserRoleMapping entity);
    Task DeleteAsync(Guid id);
}
