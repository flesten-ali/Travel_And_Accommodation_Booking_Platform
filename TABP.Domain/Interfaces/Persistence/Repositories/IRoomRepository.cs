using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface IRoomRepository : IRepository<Room>
{
    Task<PaginatedResponse<RoomForAdminResult>> GetRoomsForAdminAsync(
        Func<IQueryable<Room>, IOrderedQueryable<Room>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}
