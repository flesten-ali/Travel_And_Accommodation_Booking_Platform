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
            Thumbnail = context.Images.Where(img => img.ImageableId == h.Id && img.ImageType == ImageType.Thumbnail).FirstOrDefault()
        });

        var resultToReturn = selectedResult.GetRequestedPage(pageNumber, pageSize);
        var paginationMetaData = await resultToReturn.GetPaginationMetaDataAsync(pageNumber, pageSize);

        return new PaginatedList<SearchHotelResult>(await resultToReturn.ToListAsync(), paginationMetaData);
    }
}