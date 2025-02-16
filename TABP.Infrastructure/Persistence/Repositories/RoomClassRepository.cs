using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
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

        var requestedPage = roomClasses.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber);

        return new PaginatedList<RoomClass>(await requestedPage.ToListAsync(), paginationMetaData);
    }

    public async Task<PaginatedList<RoomClassForAdminResult>> GetRoomClassesForAdminAsync(int pageSize, int pageNumber)
    {
        var roomClasses = DbSet.Select(rc => new RoomClassForAdminResult
        {
            Description = rc.Description,
            Id = rc.Id,
            AdultsCapacity = rc.AdultsCapacity,
            ChildrenCapacity = rc.ChildrenCapacity,
            Name = rc.Name,
            NumberOfRooms = rc.Rooms.Count(),
            RoomType = rc.RoomType,
        });
        var requestedPage = roomClasses.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaDate = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber);

        return new PaginatedList<RoomClassForAdminResult>(await requestedPage.ToListAsync(), paginationMetaDate);
    }
}