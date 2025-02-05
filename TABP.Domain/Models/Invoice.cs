using TABP.Domain.Enums;

namespace TABP.Domain.Models;
public class Invoice
{
    public Guid InvoiceId { get; set; } = Guid.NewGuid();
    public DateTime IssueDate { get; set; } = DateTime.Now;
    public string HotelAddress { get; set; }
    public double TotalPrice { get; set; }
    public ICollection<RoomDetails> RoomDetails { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
}