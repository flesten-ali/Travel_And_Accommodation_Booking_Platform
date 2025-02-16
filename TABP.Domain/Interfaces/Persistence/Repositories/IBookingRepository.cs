using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<RecentlyVisitedHotelsResult>> GetRecentlyVisitedHotelsAsync(Guid guestId, int limit);
    Task<IEnumerable<TrendingCitiesResult>> GetTrendingCitiesAsync(int limit);
}
