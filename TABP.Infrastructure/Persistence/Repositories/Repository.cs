using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Common;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic repository class that provides basic CRUD operations for entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the entity being managed by the repository. The entity must implement <see cref="IEntityBase{Guid}"/>.
/// </typeparam>
public class Repository<T> : IRepository<T> where T : class, IEntityBase<Guid>, new()
{
    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;
    protected DbSet<T> DbSet => _dbSet ??= _context.Set<T>();

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously creates a new entity in the database.
    /// </summary>
    /// <param name="entity">The entity to be created.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
        {
            ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
        }
        await DbSet.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Asynchronously checks if any entity matches the given predicate.
    /// </summary>
    /// <param name="predicate">The condition to check for the existence of the entity.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous existence check, returning true if any entity matches the predicate; otherwise, false.
    /// </returns>
    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves all entities with the specified IDs.
    /// </summary>
    /// <param name="Ids">A collection of GUIDs representing the entity IDs to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous retrieval operation, returning the collection of entities.</returns>
    public async Task<IEnumerable<T>> GetAllByIdAsync(
        IEnumerable<Guid> Ids,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(entity => Ids.Contains(entity.Id)).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the entity if found; otherwise, null.</returns>
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves an entity by its ID, including related properties specified by the caller.
    /// </summary>
    /// <param name="entityId">The ID of the entity to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <param name="includeProperties">The properties to include in the query, allowing eager loading of related entities.</param>
    /// <returns>A task representing the asynchronous operation, returning the entity if found; otherwise, null.</returns>
    public async Task<T?> GetByIdIncludePropertiesAsync(
        Guid entityId,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query = DbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        var entity = await query.FirstOrDefaultAsync(entity => entity.Id == entityId, cancellationToken);

        if (entity == null) return null;

        // Special case for Hotel entities to include images (thumbnail and gallery)
        if (entity is Hotel hotel)
        {
            var thumbnail = await _context.Images
                .FirstOrDefaultAsync(img => img.ImageableId == hotel.Id && img.ImageType == ImageType.Thumbnail, cancellationToken);

            var gallery = await _context.Images
                .Where(img => img.ImageableId == hotel.Id && img.ImageType == ImageType.Gallery).ToListAsync(cancellationToken);

            if (thumbnail != null)
            {
                hotel.Thumbnail = thumbnail;
            }

            if (gallery != null)
            {
                hotel.Gallery = gallery;
            }
        }

        return entity;
    }

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    public void Delete(Guid id)
    {
        var entity = new T { Id = id };

        DbSet.Remove(entity);
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
        {
            ((IAuditEntity)entity).UpdatedDate = DateTime.Now;
        }

        DbSet.Update(entity);
    }
}