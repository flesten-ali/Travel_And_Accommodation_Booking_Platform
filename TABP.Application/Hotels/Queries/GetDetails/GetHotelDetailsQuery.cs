using MediatR;
namespace TABP.Application.Hotels.Queries.GetDetails;

public class GetHotelDetailsQuery : IRequest<GetHotelDetailsResponse>
{
    public Guid HotelId { get; set; }
}
