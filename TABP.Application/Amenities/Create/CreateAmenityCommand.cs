using MediatR;
namespace TABP.Application.Amenities.Create;

public class CreateAmenityCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
