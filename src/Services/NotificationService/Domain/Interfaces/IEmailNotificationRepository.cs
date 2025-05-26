using NotificationService.Domain.Entities;
namespace NotificationService.Domain.Interfaces;

public interface IEmailNotificationRepository
{
    Task<EmailNotification?> GetByIdAsync(Guid id);
    Task<IEnumerable<EmailNotification>> GetAllAsync();
    Task AddAsync(EmailNotification entity);
    Task UpdateAsync(EmailNotification entity);
    Task DeleteAsync(Guid id);
}
