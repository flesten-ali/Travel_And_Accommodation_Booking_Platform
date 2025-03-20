using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForHotel;
public class GetHotelRoomClassesQuery : IRequest<PaginatedResponse<HotelRoomClassesQueryResponse>>
{
    public Guid HotelId { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
}