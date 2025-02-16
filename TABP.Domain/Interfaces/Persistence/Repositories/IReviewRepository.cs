using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<PaginatedList<Review>?> GetByHotelIdAsync(
        Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy,
        Guid hotelId,
        int pageSize,
        int pageNumber);
}