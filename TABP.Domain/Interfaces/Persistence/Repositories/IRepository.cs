using System.Linq.Expressions;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllByIdAsync(IEnumerable<Guid> Ids);
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByIdIncludeProperties(Guid entityId, params Expression<Func<T, object>>[] includeProperties);
    void DeleteAsync(T entity);
}