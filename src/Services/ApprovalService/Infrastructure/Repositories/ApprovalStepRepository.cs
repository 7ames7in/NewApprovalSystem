using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Interfaces;
using ApprovalService.Infrastructure.Persistence;

public class ApprovalStepRepository<T> : IRepository<T> where T : class
{
    private readonly ApprovalDbContext _context;

    // Constructor to initialize the repository with the database context
    public ApprovalStepRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    // Retrieves an entity by its unique identifier
    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);

    // Retrieves all entities of type T
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();

    // Adds a new entity to the database
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // Updates an existing entity in the database
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    // Deletes an entity by its unique identifier
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    // Saves changes made to the database context
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Finds an entity by a specific string value (e.g., "Name" property)
    public async Task<T?> FindByAsync(string value)
    {
        // Customize this method based on your entity's properties
        return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == value);
    }
}
