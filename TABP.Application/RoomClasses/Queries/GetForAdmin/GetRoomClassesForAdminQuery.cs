using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForAdmin;
public class GetRoomClassesForAdminQuery : IRequest<PaginatedList<RoomClassForAdminResponse>>
{
    public PaginationParameters PaginationParameters { get; set; }
}
