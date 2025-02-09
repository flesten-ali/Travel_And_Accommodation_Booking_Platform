using System.Text.Json.Serialization;
using TABP.Domain.Enums;

namespace TABP.Presentation.DTOs.Booking;
public class CreateBookingRequest
{
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string? Remarks { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<Guid> RoomIds { get; set; } = [];
}
