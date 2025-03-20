using TABP.Domain.Common;

namespace TABP.Domain.Entities;

public class CartItem : EntityBase<Guid>
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public RoomClass RoomClass { get; set; }
    public Guid RoomClassId { get; set; }
}