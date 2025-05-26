using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;

public class EmailNotificationRepository : IEmailNotificationRepository
{
    private readonly DbContext _context;

    public EmailNotificationRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<EmailNotification?> GetByIdAsync(Guid id) =>
        await _context.Set<EmailNotification>().FindAsync(id);

    public async Task<IEnumerable<EmailNotification>> GetAllAsync() =>
        await _context.Set<EmailNotification>().ToListAsync();

    public async Task AddAsync(EmailNotification entity)
    {
        await _context.Set<EmailNotification>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailNotification entity)
    {
        _context.Set<EmailNotification>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<EmailNotification>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<EmailNotification>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
