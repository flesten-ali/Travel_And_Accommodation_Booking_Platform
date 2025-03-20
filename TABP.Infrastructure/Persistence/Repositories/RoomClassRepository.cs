using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class RoomClassRepository(AppDbContext context) : Repository<RoomClass>(context), IRoomClassRepository
{
    /// <summary>
    /// Asynchronously retrieves a paginated list of room classes for a specific hotel, ordered by the provided criteria.
    /// </summary>
    /// <param name="orderBy">A function to apply sorting to the room classes.</param>
    /// <param name="hotelId">The ID of the hotel for which room classes are being retrieved.</param>
    /// <param name="pageSize">The number of room classes to return per page.</param>
    /// <param name="pageNumber">The page number of results to return.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning a paginated response with the room classes and pagination metadata.</returns>
    public async Task<PaginatedResponse<RoomClass>> GetByHotelIdAsync(
        Func<IQueryable<RoomClass>, IOrderedQueryable<RoomClass>> orderBy,
        Guid hotelId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var filteredRoomClasses = DbSet
            .Include(rc => rc.Discounts)
            .Include(rc => rc.Amenities)
            .Where(rc => rc.HotelId == hotelId)
            .AsNoTracking();

        var roomClasses = orderBy(filteredRoomClasses);

        var requestedPage = roomClasses.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<RoomClass>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }

    /// <summary>
    /// Asynchronously retrieves a paginated list of room classes for admin, ordered by the provided criteria.
    /// </summary>
    /// <param name="orderBy">A function to apply sorting to the room classes.</param>
    /// <param name="pageSize">The number of room classes to return per page.</param>
    /// <param name="pageNumber">The page number of results to return.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning a paginated response with the room classes and pagination metadata.
    /// </returns>
    public async Task<PaginatedResponse<RoomClassForAdminResult>> GetRoomClassesForAdminAsync(
        Func<IQueryable<RoomClass>, IOrderedQueryable<RoomClass>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var allRoomClasses = DbSet.AsNoTracking();

        var roomClasses = orderBy(allRoomClasses).Select(rc => new RoomClassForAdminResult 
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
        var paginationMetaDate = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<RoomClassForAdminResult>(await requestedPage.ToListAsync(cancellationToken), paginationMetaDate);
    }
}