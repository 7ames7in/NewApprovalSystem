public interface IUserRoleRepository
{
    Task<UserRole?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserRole>> GetAllAsync();
    Task AddAsync(UserRole entity);
    Task UpdateAsync(UserRole entity);
    Task DeleteAsync(Guid id);
}
