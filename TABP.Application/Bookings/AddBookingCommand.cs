using TABP.Domain.Entities;
using TABP.Domain.Enums;

namespace TABP.Application.Bookings;

public class AddBookingCommand
{
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }
    public string Remarks { get; set; }
    public decimal TotalPrice { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public ICollection<Room> Rooms { get; set; } = [];
}
