using TABP.Domain.Common;
using TABP.Domain.Enums;
namespace TABP.Domain.Entities;

public class Invoice : EntityBase<Guid>
{
    public DateTime IssueDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public double TotalPrice { get; set; }
    public Booking Booking { get; set; }
    public Guid BookingId { get; set; }
}
