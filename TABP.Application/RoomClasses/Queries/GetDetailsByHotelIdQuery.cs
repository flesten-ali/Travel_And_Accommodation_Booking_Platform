using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries;
public class GetDetailsByHotelIdQuery : IRequest<PaginatedList<GetDetailsByHotelIdQueryResponse>>
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}