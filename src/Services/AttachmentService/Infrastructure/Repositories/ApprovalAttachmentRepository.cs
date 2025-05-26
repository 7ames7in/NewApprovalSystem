public class ApprovalAttachmentRepository : IApprovalAttachmentRepository
{
    private readonly DbContext _context;

    public ApprovalAttachmentRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<ApprovalAttachment?> GetByIdAsync(Guid id) =>
        await _context.Set<ApprovalAttachment>().FindAsync(id);

    public async Task<IEnumerable<ApprovalAttachment>> GetAllAsync() =>
        await _context.Set<ApprovalAttachment>().ToListAsync();

    public async Task AddAsync(ApprovalAttachment entity)
    {
        await _context.Set<ApprovalAttachment>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApprovalAttachment entity)
    {
        _context.Set<ApprovalAttachment>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<ApprovalAttachment>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<ApprovalAttachment>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
