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
    public Task<Hotel?> GetHotelByIdAsync(Guid hotelId, params Expression<Func<Hotel, object>>[] includeProperties)
    {
        var hotel = DbSet.Where(h => h.Id == hotelId);

        foreach (var includeProperty in includeProperties)
        {
            hotel = hotel.Include(includeProperty);
        }

        return hotel.FirstOrDefaultAsync();
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