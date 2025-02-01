namespace TABP.Domain.Entities;

public class Amenity : EntityBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<RoomClass> RoomClasses { get; set; } = [];
}
