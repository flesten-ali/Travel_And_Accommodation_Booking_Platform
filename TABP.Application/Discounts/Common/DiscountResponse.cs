namespace TABP.Application.Discounts.Common;
public class DiscountResponse
{
    public Guid Id { get; set; }
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomClassId { get; set; }
}
