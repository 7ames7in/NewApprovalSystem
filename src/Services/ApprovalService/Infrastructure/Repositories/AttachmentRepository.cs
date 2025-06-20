using System.Threading.Tasks;
using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Interfaces;
using ApprovalService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Repositories;

public class AttachmentRepository<T> : IAttachmentRepository<T> where T : ApprovalAttachment
{
    private readonly ApprovalDbContext _context;

    public AttachmentRepository(ApprovalDbContext context)
    {
        _context = context;
    }

     public Task AddAsync(T entity)
     {
          throw new NotImplementedException();
     }

     public Task DeleteAsync(Guid id)
     {
          throw new NotImplementedException();
     }

     public async Task DeleteAttachment(string attachmentId)
    {
        var attachment = await GetAttachmentByIdAsync(attachmentId);
        if (attachment != null)
        {
            _context.Set<T>().Remove(attachment);
            _context.SaveChanges();
        }
    }

     public Task<IEnumerable<T>> GetAllAsync()
     {
          throw new NotImplementedException();
     }

     public async Task<T> GetAttachmentByIdAsync(string attachmentId)
    {
        Guid.TryParse(attachmentId, out var guidAttachmentId);
        if (guidAttachmentId == Guid.Empty)
        {
            throw new ArgumentException("Invalid attachment ID format.", nameof(attachmentId));
        }
        var attachment = await _context.Set<T>().FindAsync(guidAttachmentId);  
        return attachment ?? throw new KeyNotFoundException($"Attachment with ID {attachmentId} not found.");
    }

     public Task<T?> GetByIdAsync(Guid id)
     {
          throw new NotImplementedException();
     }

     public Task SaveChangesAsync()
     {
          throw new NotImplementedException();
     }

     public Task UpdateAsync(T entity)
     {
          throw new NotImplementedException();
     }
}