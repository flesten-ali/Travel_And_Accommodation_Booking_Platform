using Microsoft.EntityFrameworkCore;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbFactory _dbFactory;
    private DbSet<T> _dbSet;
    protected DbSet<T> DbSet => _dbSet ??= _dbFactory.DbContext.Set<T>();

    public Repository(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }
}
