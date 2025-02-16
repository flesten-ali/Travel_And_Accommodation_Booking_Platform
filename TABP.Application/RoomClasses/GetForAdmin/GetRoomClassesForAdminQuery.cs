using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForAdmin;
public class GetRoomClassesForAdminQuery : IRequest<PaginatedList<RoomClassForAdminResponse>>
{
    public PaginationParameters PaginationParameters { get; set; }
}
