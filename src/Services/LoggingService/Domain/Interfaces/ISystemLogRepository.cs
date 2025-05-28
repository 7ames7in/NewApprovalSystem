using LoggingService.Domain.Entities;

//사용안함.
public interface ISystemLogRepository
{
    Task<SystemLog?> GetByIdAsync(Guid id);
    Task<IEnumerable<SystemLog>> GetAllAsync();
    Task AddAsync(SystemLog entity);
    Task UpdateAsync(SystemLog entity);
    Task DeleteAsync(Guid id);
}
