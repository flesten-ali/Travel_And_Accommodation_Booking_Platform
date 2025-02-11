using MediatR;
using TABP.Application.Amenities.Common;
namespace TABP.Application.Amenities.Commands.Create;

public class CreateAmenityCommand : IRequest<AmenityResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
