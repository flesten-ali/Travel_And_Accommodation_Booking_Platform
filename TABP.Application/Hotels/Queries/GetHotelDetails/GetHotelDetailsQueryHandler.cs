using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetHotelDetails;
public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQuery, GetHotelDetailsResponse>
{
    private readonly IHotelRepository _hotelRepository;

    public GetHotelDetailsQueryHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }
    public async Task<GetHotelDetailsResponse> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
    {
        

        return new GetHotelDetailsResponse();






    }
}
