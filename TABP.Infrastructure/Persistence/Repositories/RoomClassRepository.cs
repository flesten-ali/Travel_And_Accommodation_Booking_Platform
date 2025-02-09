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
            .Select(rc => new
            {
                roomClass = rc,
                discountedPrice = CalculateRoomClassPriceAfterDiscount(rc.Price, rc.Discounts)
            })
             .OrderByDescending(x => x.discountedPrice)
             .Take(NumberOfDeals)
             .Select(x => new FeaturedDealResult
             {
                 Description = x.roomClass.Hotel.Description,
                 CityName = x.roomClass.Hotel.City.Name,
                 Id = x.roomClass.Hotel.Id,
                 Name = x.roomClass.Hotel.Name,
                 StarRate = x.roomClass.Hotel.Rate,
                 ThumbnailUrl = context.Images
                               .Where(img => img.ImageableId == x.roomClass.Hotel.Id && img.ImageType == ImageType.Thumbnail)
                               .Select(img => img.ImageUrl)
                               .FirstOrDefault() ?? "",
                 DiscountedPrice = x.discountedPrice,
                 OriginalPrice = x.roomClass.Price,
             }).ToListAsync();

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