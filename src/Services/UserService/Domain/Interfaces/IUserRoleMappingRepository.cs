using UserService.Domain.Entities;
using BuildingBlocks.Core.Interfaces;

namespace UserService.Domain.Interfaces;


//하지만 지금은 사용하지 않음. 공통으로 뺐음.

public interface IUserRoleMappingRepository<T> : IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetRolesByUserIdAsync(string userId);
    Task<T?> GetRoleByIdAsync(Guid roleId);
    
}
