using MediatR;
namespace TABP.Application.Hotels.Queries.GetDetails;

public class GetHotelQuery : IRequest<HotelDetailsResponse>
{
    public Guid HotelId { get; set; }
}
