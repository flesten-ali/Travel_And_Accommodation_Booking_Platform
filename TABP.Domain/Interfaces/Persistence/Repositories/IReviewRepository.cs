using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<PaginatedList<Review>?> GetByHotelIdAsync(Guid hotelId, int pageSize, int pageNumber);
}
