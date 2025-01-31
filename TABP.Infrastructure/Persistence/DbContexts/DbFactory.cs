using Microsoft.EntityFrameworkCore;

namespace TABP.Infrastructure.Persistence.DbContexts;
public class DbFactory : IDisposable
{
    private readonly Func<AppDbContext> _instanceFunc;
    private DbContext _dbContext;
    private bool _disposed;
    public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

    public DbFactory(Func<AppDbContext> dbContextFactory)
    {
        _instanceFunc = dbContextFactory;
    }

    public void Dispose()
    {
        if (!_disposed && _dbContext != null)
        {
            _disposed = true;
            _dbContext.Dispose();
        }
    }
}
