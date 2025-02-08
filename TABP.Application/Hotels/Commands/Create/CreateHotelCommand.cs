using MediatR;
namespace TABP.Application.Hotels.Commands.Create;

public class CreateHotelCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Rate { get; set; }
    public double LongitudeCoordinates { get; set; }
    public double LatitudeCoordinates { get; set; }
    public Guid CityId { get; set; }
    public Guid OwnerId { get; set; }
}
