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
    {
        var recentlyVisitedHotels = await DbSet
            .Where(b => b.UserId == guestId)
            .Include(b => b.Invoice)
            .Include(b => b.Rooms)
            .ThenInclude(r => r.RoomClass)
            .ThenInclude(rc => rc.Hotel)
            .GroupBy(b => b.Rooms.FirstOrDefault()!.RoomClass.Hotel)
            .Select(g => new
            {
                hotel = g.Key,
                recentBookingInThisHotel = g.OrderByDescending(b => b.BookingDate).FirstOrDefault()!,
            })
            .OrderByDescending(x => x.recentBookingInThisHotel.BookingDate)
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
                BookingDate = x.recentBookingInThisHotel.BookingDate,
                CheckInDate = x.recentBookingInThisHotel.CheckInDate,
                CheckOutDate = x.recentBookingInThisHotel.CheckOutDate,
                BookingId = x.recentBookingInThisHotel.Id,
                Price = x.recentBookingInThisHotel.Invoice.TotalPrice
            })
            .AsNoTracking()
            .ToListAsync();

        return recentlyVisitedHotels;
    }
}
