using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface ICartItemRepository : IRepository<CartItem>
{
    Task<PaginatedResponse<CartItem>> GetCartItemsAsync(
        Func<IQueryable<CartItem>, IOrderedQueryable<CartItem>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}
