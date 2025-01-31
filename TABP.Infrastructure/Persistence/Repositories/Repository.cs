using Microsoft.EntityFrameworkCore;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
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
}
