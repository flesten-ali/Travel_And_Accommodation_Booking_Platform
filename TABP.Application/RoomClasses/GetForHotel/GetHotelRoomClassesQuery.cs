using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForHotel;
public class GetHotelRoomClassesQuery : IRequest<PaginatedList<HotelRoomClassesQueryResponse>>
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}