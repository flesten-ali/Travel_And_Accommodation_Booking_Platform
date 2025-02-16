using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extenstions;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class HotelRepository(AppDbContext context) : Repository<Hotel>(context), IHotelRepository
{
    public async Task<PaginatedList<SearchHotelResult>> SearchHotelsAsync(
        Expression<Func<Hotel, bool>> filter,
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy,
        int pageSize,
        int pageNumber)
    {
        var hotels = DbSet.Where(filter);
        hotels = orderBy(hotels);

        var selectedResult = hotels.Select(h => new SearchHotelResult
        {
            Name = h.Name,
            Description = h.Description,
            StarRating = h.Rate,
            PricePerNight = h.RoomClasses.Count != 0 ? h.RoomClasses.Min(rc => rc.Price) : 0,
            ThumbnailUrl = context.Images
                                  .Where(img => img.ImageableId == h.Id && img.ImageType == ImageType.Thumbnail)
                                  .Select(img => img.ImageUrl)
                                  .FirstOrDefault() ?? ""
        });

        var resultToReturn = selectedResult.GetRequestedPage(pageNumber, pageSize);
        var paginationMetaData = await resultToReturn.GetPaginationMetaDataAsync(pageNumber, pageSize);

        return new PaginatedList<SearchHotelResult>(await resultToReturn.ToListAsync(), paginationMetaData);
    }

    public async Task<IEnumerable<FeaturedDealResult>> GetFeaturedDealsAsync(int NumberOfDeals)
    {
        var currentDate = DateTime.UtcNow;

        var featuredDeals = await DbSet
            .Include(h => h.City)
            .Include(h => h.RoomClasses)
               .ThenInclude(rc => rc.Discounts)
            .Where(h => h.RoomClasses.Count != 0)
            .Select(h => new
            {
                Hotel = h,
                DiscountedPrice = h.RoomClasses.Min(rc =>
                    rc.Discounts.Count != 0
                    ? rc.Price * (1 - (rc.Discounts.Where(d => d.StartDate <= currentDate && d.EndDate > currentDate).Max(d => d.Percentage) / 100))
                    : rc.Price)
            })
            .OrderBy(x => x.DiscountedPrice)
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
                DiscountedPrice = x.DiscountedPrice,
                OriginalPrice = x.Hotel.RoomClasses.Max(rc => rc.Price),
            })
            .ToListAsync();
        return featuredDeals;
    }

    public async Task<PaginatedList<HotelForAdminResult>> GetHotelsForAdminAsync(int pageSize, int pageNumber)
    {
        var hotels = DbSet.Select(h => new HotelForAdminResult
        {
            Id = h.Id,
            CityName = h.City.Name,
            Name = h.Name,
            Rate = h.Rate,
            CreatedDate = h.CreatedDate,
            UpdatedDate = h.UpdatedDate,
            OwnerName = h.Owner.Name,
        }).AsNoTracking();

        var requestedPage = PaginationExtenstions.GetRequestedPage(hotels, pageNumber, pageSize);
        var paginationMetaDate = await requestedPage.GetPaginationMetaDataAsync(pageNumber, pageSize);
        return new PaginatedList<HotelForAdminResult>(await hotels.ToListAsync(), paginationMetaDate);
    }
}