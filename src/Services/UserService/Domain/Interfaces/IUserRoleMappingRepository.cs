public interface IUserRoleMappingRepository
{
    Task<UserRoleMapping?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserRoleMapping>> GetAllAsync();
    Task AddAsync(UserRoleMapping entity);
    Task UpdateAsync(UserRoleMapping entity);
    Task DeleteAsync(Guid id);
}
