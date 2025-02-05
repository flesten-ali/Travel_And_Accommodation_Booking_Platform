using TABP.Domain.Enums;
namespace TABP.Domain.Entities;

public class Booking : EntityBase<Guid>
{
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }
    public string? Remarks { get; set; }
    public double TotalPrice { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Room> Rooms { get; set; } = [];
}
