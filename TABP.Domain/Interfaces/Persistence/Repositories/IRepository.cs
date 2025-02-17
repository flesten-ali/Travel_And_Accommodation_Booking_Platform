using System.Linq.Expressions;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IRepository<T> where T : class, new()
{
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllByIdAsync(IEnumerable<Guid> Ids, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<T?> GetByIdIncludePropertiesAsync(
        Guid entityId,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties);

    void Delete(Guid id);

    void Update(T entity);
}