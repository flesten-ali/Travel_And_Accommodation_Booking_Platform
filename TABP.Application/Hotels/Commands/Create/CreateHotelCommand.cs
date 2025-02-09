using MediatR;
using TABP.Application.Hotels.Common;
namespace TABP.Application.Hotels.Commands.Create;

public class CreateHotelCommand : IRequest<HotelResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Rate { get; set; }
    public double LongitudeCoordinates { get; set; }
    public double LatitudeCoordinates { get; set; }
    public Guid CityId { get; set; }
    public Guid OwnerId { get; set; }
}
