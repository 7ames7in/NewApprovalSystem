using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Core.Interfaces;
using ApprovalService.Infrastructure.Persistence;

namespace ApprovalService.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation for managing entities in the ApprovalDbContext.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class ApprovalTemplateRepository<T> : IRepository<T> where T : class
{
    private readonly ApprovalDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApprovalTemplateRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ApprovalTemplateRepository(ApprovalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Saves changes made to the database context.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
