using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface IRoomClassRepository : IRepository<RoomClass>
{
    Task<PaginatedList<RoomClass>> GetByHotelIdAsync(
        Func<IQueryable<RoomClass>, IOrderedQueryable<RoomClass>> orderBy,
        Guid hotelId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<RoomClassForAdminResult>> GetRoomClassesForAdminAsync(
        Func<IQueryable<RoomClass>, IOrderedQueryable<RoomClass>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}