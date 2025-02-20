using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface IDiscountRepository : IRepository<Discount>
{
    Task<PaginatedResponse<Discount>> GetDiscountsForRoomClass(
        Func<IQueryable<Discount>, IOrderedQueryable<Discount>> orderBy,
        Guid roomClassId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}