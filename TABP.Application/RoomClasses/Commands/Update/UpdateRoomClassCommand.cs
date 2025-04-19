using MediatR;
using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Commands.Update;
public class UpdateRoomClassCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public double Price { get; set; }
    public Guid HotelId { get; set; }
    public IEnumerable<Guid> AmenityIds { get; set; } = [];
}
