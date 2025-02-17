using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<PaginatedList<SearchHotelResult>> SearchHotelsAsync(
        Expression<Func<Hotel, bool>> filter,
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy,
        int pageSize,
        int pageCount,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<FeaturedDealResult>> GetFeaturedDealsAsync(
        int limit,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<HotelForAdminResult>> GetHotelsForAdminAsync(
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}
