using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForHotel;
public class GetHotelRoomClassesQuery : IRequest<PaginatedList<HotelRoomClassesQueryResponse>>
{
    public Guid HotelId { get; set; }
    public PaginationParameters PaginationParameters  { get; set; }
}