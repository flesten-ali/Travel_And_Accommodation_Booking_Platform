using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries;
public class GetDetailsByHotelIdQueryHandler
    : IRequestHandler<GetDetailsByHotelIdQuery, PaginatedList<GetDetailsByHotelIdQueryResponse>>
{
    public Task<PaginatedList<GetDetailsByHotelIdQueryResponse>> Handle(
        GetDetailsByHotelIdQuery request, 
    CancellationToken cancellationToken
    )
    {

    }
}
