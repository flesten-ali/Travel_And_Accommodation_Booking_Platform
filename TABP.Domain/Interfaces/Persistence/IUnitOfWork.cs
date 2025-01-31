namespace TABP.Domain.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChangesAsync();
}
