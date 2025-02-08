namespace TABP.Domain.Entities;

public class CartItem : EntityBase<Guid>
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string? Remarks { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public RoomClass RoomClass { get; set; }
    public Guid RoomClassId { get; set; }
}