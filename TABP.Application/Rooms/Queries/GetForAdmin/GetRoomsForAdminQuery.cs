using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.Rooms.Queries.GetForAdmin;
public class GetRoomsForAdminQuery : IRequest<PaginatedResponse<RoomForAdminResponse>>
{
    public Guid RoomClassId { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
}
