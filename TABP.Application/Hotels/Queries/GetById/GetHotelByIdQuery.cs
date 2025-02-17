using MediatR;
using TABP.Application.Hotels.Common;
namespace TABP.Application.Hotels.Queries.GetById;

public class GetHotelByIdQuery : IRequest<HotelResponse>
{
    public Guid HotelId { get; set; }
}
