using MediatR;
namespace TABP.Application.Amenities.Add;

public class AddAmenityCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
