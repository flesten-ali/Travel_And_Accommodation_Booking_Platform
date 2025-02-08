using TABP.Domain.Enums;
namespace TABP.Application.Bookings.Common;

public class BookingResponse
{
    public Guid Id { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }
    public string? Remarks { get; set; }
    public decimal TotalPrice { get; set; } 
    public Guid UserId { get; set; } 
    public IEnumerable<Guid> RoomIds { get; set; } = []; 
}
