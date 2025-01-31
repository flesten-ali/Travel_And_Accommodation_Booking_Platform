using Microsoft.EntityFrameworkCore.Storage;
using TABP.Domain.Interfaces.Persistence;
namespace TABP.Infrastructure.Persistence.DbContexts;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbFactory _dbFactory;
    private IDbContextTransaction _transaction;

    public UnitOfWork(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public void BeginTransaction()
    {
        _transaction = _dbFactory.DbContext.Database.BeginTransaction();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _dbFactory.DbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }

    public Task<int> SaveChangesAsync()
    {
        return _dbFactory.DbContext.SaveChangesAsync();
    }
}