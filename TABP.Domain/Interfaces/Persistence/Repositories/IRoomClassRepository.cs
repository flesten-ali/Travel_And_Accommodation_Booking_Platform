using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface IRoomClassRepository : IRepository<RoomClass>
{
    Task<PaginatedList<RoomClass>> GetByHotelIdAsync(Guid hotelId, int pageSize, int pageNumber);
}