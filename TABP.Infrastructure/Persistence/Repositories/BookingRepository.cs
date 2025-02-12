using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class BookingRepository(AppDbContext context) : Repository<Booking>(context), IBookingRepository
{
    public async Task<IEnumerable<RecentlyVisitedHotelsResult>> GetRecentlyVisitedHotels(Guid guestId, int limit)
    {//
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
                         .ToListAsync();

        var recentlyVisitedHotels = query
                          .DistinctBy(x => x.hotel.Id)
                          .Take(limit)
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
                          .ToList();

        return recentlyVisitedHotels;
    }

    public async Task<IEnumerable<TrendingCitiesResult>> GetTrendingCities(int limit)
    {
        var trendingHotels = await DbSet
            .Include(b => b.Rooms)
            .ThenInclude(r => r.RoomClass)
            .ThenInclude(rc => rc.Hotel)
            .ThenInclude(h => h.City)
            .SelectMany(b => b.Rooms, (b, r) => new
            {
                room = r
            })
            .GroupBy(x => x.room.RoomClass.Hotel.City)
            .OrderByDescending(g => g.Count())
            .Take(limit)
            .Select(g => new
            {
                city = g.Key
            })
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
            .ToListAsync();

        return trendingHotels;
    }
}
