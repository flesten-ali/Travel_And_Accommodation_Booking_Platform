using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extenstions;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class HotelRepository(AppDbContext context) : Repository<Hotel>(context), IHotelRepository
{
    public async Task<Hotel?> GetHotelByIdAsync(Guid hotelId)
    {
        var hotel = await DbSet.Where(h => h.Id == hotelId)
                          .Include(h => h.Reviews)
                          .ThenInclude(r => r.User)
                          .Include(h => h.RoomClasses)
                          .Include(h => h.City)
                          .Include(h => h.Gallery)
                          .FirstOrDefaultAsync();
        return hotel;
    }

    public async Task<PaginatedList<SearchHotelResult>> SearchHotels(
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
            PricePerNight = h.RoomClasses.Min(rc => rc.Price),
            Thumbnail = context.Images.Where(img => img.Id == h.ThumbnailId && img.ImageableId == h.Id).FirstOrDefault()
        });

        var resultToReturn = selectedResult.GetRequestedPage(pageNumber, pageSize);
        var paginationMetaData = await resultToReturn.GetPaginationMetaDataAsync(pageNumber, pageSize);
        return new PaginatedList<SearchHotelResult>(resultToReturn, paginationMetaData);
    }
}