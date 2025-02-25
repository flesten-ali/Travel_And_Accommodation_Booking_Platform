using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Date;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class RoomRepository(AppDbContext context, IDateTimeProvider dateTimeProvider) : Repository<Room>(context), IRoomRepository
{
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Asynchronously retrieves a paginated list of rooms for administrative purposes, ordered by the specified criteria.
    /// </summary>
    /// <param name="orderBy">A function to apply sorting to the rooms.</param>
    /// <param name="pageSize">The number of rooms to return per page.</param>
    /// <param name="pageNumber">The page number of results to return.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning a paginated response with room details for admin.</returns>
    public async Task<PaginatedResponse<RoomForAdminResult>> GetRoomsForAdminAsync(
        Func<IQueryable<Room>, IOrderedQueryable<Room>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var currentDate = _dateTimeProvider.UtcNow;
        var allRooms = DbSet.AsNoTracking();

        var rooms = orderBy(allRooms).Select(room => new RoomForAdminResult(default, default, default, default)
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
