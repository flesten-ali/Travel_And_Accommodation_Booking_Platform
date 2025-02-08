using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Entities;
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

    public Task<T?> GetByIdIncludeProperties(Guid entityId, params Expression<Func<T, object>>[] includeProperties)
    {
        var entity = DbSet.Where(e => e.Id == entityId);

        foreach (var includeProperty in includeProperties)
        {
            entity = entity.Include(includeProperty);
        }

        return entity.FirstOrDefaultAsync();
    }
}