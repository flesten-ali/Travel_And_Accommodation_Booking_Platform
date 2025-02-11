using MediatR;
namespace TABP.Application.Amenities.Create;

public class CreateAmenityCommand : IRequest<AmenityResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
