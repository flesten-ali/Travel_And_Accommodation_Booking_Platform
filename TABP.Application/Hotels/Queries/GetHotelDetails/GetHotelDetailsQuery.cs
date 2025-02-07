using MediatR;

namespace TABP.Application.Hotels.Queries.GetHotelDetails;
public class GetHotelDetailsQuery : IRequest<GetHotelDetailsResponse>
{
    public Guid HotelId { get; set; }
}
