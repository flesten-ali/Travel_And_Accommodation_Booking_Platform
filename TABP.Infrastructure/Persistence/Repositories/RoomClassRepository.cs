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
}