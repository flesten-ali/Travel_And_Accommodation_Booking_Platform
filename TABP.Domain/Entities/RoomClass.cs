using TABP.Domain.Enums;
namespace TABP.Domain.Entities;

public class RoomClass : EntityBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public double Price { get; set; }
    public ICollection<Image> Gallery { get; set; }
    public ICollection<Discount> Discounts { get; set; }
    public Hotel Hotel { get; set; }
    public Guid HotelId { get; set; }
    public ICollection<Amenity> Amenities { get; set; } = [];
    public ICollection<Room> Rooms { get; set; } = [];
}
