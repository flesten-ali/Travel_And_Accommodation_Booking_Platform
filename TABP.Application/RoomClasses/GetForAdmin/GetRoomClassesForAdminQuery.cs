using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForAdmin;
public class GetRoomClassesForAdminQuery : IRequest<PaginatedList<RoomClassForAdminResponse>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
