using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<RecentlyVisitedHotelsResult>> GetRecentlyVisitedHotels(Guid guestId, int limit);
}
