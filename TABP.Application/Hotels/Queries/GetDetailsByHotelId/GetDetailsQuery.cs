using MediatR;
namespace TABP.Application.Hotels.Queries.GetDetailsByHotelId;

public class GetDetailsQuery : IRequest<GetDetailsResponse>
{
    public Guid HotelId { get; set; }
}
