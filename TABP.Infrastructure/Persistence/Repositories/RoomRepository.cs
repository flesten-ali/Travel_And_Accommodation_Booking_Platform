using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class RoomRepository(AppDbContext context) : Repository<Room>(context), IRoomRepository
{
    public async Task<PaginatedResponse<RoomForAdminResult>> GetRoomsForAdminAsync(
        Func<IQueryable<Room>, IOrderedQueryable<Room>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow;
        var allRooms = DbSet.AsNoTracking();

        var rooms = orderBy(allRooms).Select(room => new RoomForAdminResult
        {
            RoomNumber = room.RoomNumber,
            Floor = room.Floor,
            IsAvaiable = !context.Bookings
                   .Any(b => b.Rooms.Any(r => r.Id == room.Id) && b.CheckInDate <= currentDate && b.CheckOutDate > currentDate)
        });

        var requestedPage = rooms.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<RoomForAdminResult>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}
