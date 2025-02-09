using MediatR;
namespace TABP.Application.Hotels.Queries.GetDetails;

public class GetHotelQuery : IRequest<GetHotelResponse>
{
    public Guid HotelId { get; set; }
}
