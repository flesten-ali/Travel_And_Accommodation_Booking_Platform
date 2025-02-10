using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extenstions;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class RoomClassRepository(AppDbContext context) : Repository<RoomClass>(context), IRoomClassRepository
{
    public async Task<PaginatedList<RoomClass>> GetByHotelIdAsync(Guid hotelId, int pageSize, int pageNumber)
    {
        var roomClasses = DbSet.Where(rc => rc.HotelId == hotelId)
                                   .Include(rc => rc.Discounts)
                                   .Include(rc => rc.Amenities)
                                   .AsQueryable()
                                   .AsNoTracking();

        var requestedPage = roomClasses.GetRequestedPage(pageNumber, pageSize);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageNumber, pageSize);

        return new PaginatedList<RoomClass>(await requestedPage.ToListAsync(), paginationMetaData);
    }

    public async Task<IEnumerable<FeaturedDealResult>> GetFeaturedDeals(int NumberOfDeals)
    {
        var featuredDeals = await DbSet
            .Include(rc => rc.Discounts)
            .Include(rc => rc.Hotel)
            .ThenInclude(h => h.City)
            .GroupBy(x => x.Hotel)
            .Select(g => new
            {
                Hotel = g.Key,
                maxDiscountedPrice = g.Max(rc => CalculateRoomClassPriceAfterDiscount(rc.Price, rc.Discounts))
            })
             .OrderByDescending(x => x.maxDiscountedPrice)
             .Take(NumberOfDeals)
             .Select(x => new FeaturedDealResult
             {
                 Description = x.Hotel.Description,
                 CityName = x.Hotel.City.Name,
                 Id = x.Hotel.Id,
                 Name = x.Hotel.Name,
                 StarRate = x.Hotel.Rate,
                 ThumbnailUrl = context.Images
                               .Where(img => img.ImageableId == x.Hotel.Id && img.ImageType == ImageType.Thumbnail)
                               .Select(img => img.ImageUrl)
                               .FirstOrDefault() ?? "",
                 DiscountedPrice = x.maxDiscountedPrice,
                 OriginalPrice = x.Hotel.RoomClasses.Max(rc => rc.Price),
             })
             .AsNoTracking()
             .ToListAsync();

        return featuredDeals;
    }

    private static double CalculateRoomClassPriceAfterDiscount(double price, ICollection<Discount> discounts)
    {
        if (discounts == null || discounts.Count == 0) return price;

        var currentDate = DateTime.UtcNow;

        var maxDiscount = discounts.Where(d => d.StartDate <= currentDate && d.EndDate > currentDate).Max(d => d.Percentage);
        var priceAfterDiscount = price * (1 - maxDiscount / 100);
        return priceAfterDiscount;
    }
}