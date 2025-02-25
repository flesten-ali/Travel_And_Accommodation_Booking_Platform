using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class BookingRepository(AppDbContext context) : Repository<Booking>(context), IBookingRepository
{
    /// <summary>
    /// Retrieves a list of recently visited hotels by a guest.
    /// </summary>
    /// <param name="guestId">The unique identifier of the guest.</param>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A collection of recently visited hotels.</returns>
    public async Task<IEnumerable<RecentlyVisitedHotelsResult>> GetRecentlyVisitedHotelsAsync(
        Guid guestId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var query = await DbSet
                         .Include(b => b.Invoice)
                         .Include(b => b.Rooms)
                            .ThenInclude(r => r.RoomClass)
                              .ThenInclude(rc => rc.Hotel)
                         .OrderByDescending(b => b.BookingDate)
                         .Select(b => new
                         {
                             booking = b,
                             hotel = b.Rooms.Select(r => r.RoomClass.Hotel).FirstOrDefault()!,
                         })
                         .AsNoTracking()
                         .ToListAsync(cancellationToken);

        var recentlyVisitedHotels = query
                          .Select(x => new RecentlyVisitedHotelsResult
                          {
                              Id = x.hotel.Id,
                              Name = x.hotel.Name,
                              CityName = x.hotel.City.Name,
                              Rate = x.hotel.Rate,
                              ThumbnailUrl = context.Images
                                                .Where(img => img.ImageableId == x.hotel.Id && img.ImageType == ImageType.Thumbnail)
                                                .Select(img => img.ImageUrl)
                                                .FirstOrDefault() ?? "",
                              BookingDate = x.booking.BookingDate,
                              CheckInDate = x.booking.CheckInDate,
                              CheckOutDate = x.booking.CheckOutDate,
                              BookingId = x.booking.Id,
                              Price = x.booking.Invoice.TotalPrice
                          })
                          .DistinctBy(x => x.Id)
                          .Take(limit)
                          .ToList();

        return recentlyVisitedHotels;
    }

    /// <summary>
    /// Retrieves a list of trending cities based on booking activity.
    /// </summary>
    /// <param name="limit">The maximum number of trending cities to return.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A collection of trending cities.</returns>
    public async Task<IEnumerable<TrendingCitiesResult>> GetTrendingCitiesAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        var trendingCities = await DbSet
            .Include(b => b.Rooms)
               .ThenInclude(r => r.RoomClass)
                  .ThenInclude(rc => rc.Hotel)
                     .ThenInclude(h => h.City)
            .SelectMany(b => b.Rooms, (b, r) => new { room = r })
            .GroupBy(x => x.room.RoomClass.Hotel.City)
            .OrderByDescending(g => g.Count())
            .Take(limit)
            .Select(g => new { city = g.Key })
            .Select(x => new TrendingCitiesResult
            {
                Id = x.city.Id,
                Name = x.city.Name,
                ThumbnailUrl = context.Images
                     .Where(img => img.ImageableId == x.city.Id && img.ImageType == ImageType.Thumbnail)
                     .Select(img => img.ImageUrl)
                     .FirstOrDefault() ?? "",
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return trendingCities;
    }
}
