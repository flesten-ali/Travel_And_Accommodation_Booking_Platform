namespace TABP.Domain.Entities;

public class Room : EntityBase<Guid>
{
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public RoomClass RoomClass { get; set; }
    public Guid RoomClassId { get; set; }
    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}
