using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Exceptions;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntityBase<Guid>, new()
{
    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;
    protected DbSet<T> DbSet => _dbSet ??= _context.Set<T>();

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
        {
            ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
        }
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllByIdAsync(
        IEnumerable<Guid> Ids,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(entity => Ids.Contains(entity.Id)).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }

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

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await DbSet.AnyAsync(e => e.Id == id, cancellationToken))
            throw new NotFoundException(id);

        var entity = new T { Id = id };

        DbSet.Remove(entity);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (!await DbSet.AnyAsync(e => e.Id == entity.Id, cancellationToken))
            throw new NotFoundException(entity.Id);

        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
        {
            ((IAuditEntity)entity).UpdatedDate = DateTime.Now;
        }

        DbSet.Update(entity);
    }
}