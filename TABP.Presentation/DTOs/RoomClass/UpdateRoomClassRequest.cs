using TABP.Domain.Enums;

namespace TABP.Presentation.DTOs.RoomClass;
public class UpdateRoomClassRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public double Price { get; set; }
    public Guid HotelId { get; set; }
    public IEnumerable<Guid> AmenityIds { get; set; } = [];
}
