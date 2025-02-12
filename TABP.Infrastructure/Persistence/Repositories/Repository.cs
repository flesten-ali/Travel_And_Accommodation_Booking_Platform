using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntityBase<Guid>
{
    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;
    protected DbSet<T> DbSet => _dbSet ??= _context.Set<T>();

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        if (typeof(IAuditEntity<T>).IsAssignableFrom(typeof(T)))
        {
            ((IAuditEntity<T>)entity).CreatedDate = DateTime.UtcNow;
        }
        await DbSet.AddAsync(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllByIdAsync(IEnumerable<Guid> Ids)
    {
        return await DbSet.Where(entity => Ids.Contains(entity.Id)).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdIncludeProperties(Guid entityId, params Expression<Func<T, object>>[] includeProperties)
    {
        var query = DbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        var entity = await query.FirstOrDefaultAsync(entity => entity.Id == entityId);

        if (entity == null) return null;

        if (entity is Hotel hotel)
        {
            var thumbnail = await _context.Images
                .FirstOrDefaultAsync(img => img.ImageableId == hotel.Id && img.ImageType == ImageType.Thumbnail);

            var gallery = await _context.Images
                .Where(img => img.ImageableId == hotel.Id && img.ImageType == ImageType.Gallery).ToListAsync();

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
}