using MediatR;

namespace TABP.Application.Amenities.Commands.Update;
public class UpdateAmenityCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
