namespace TABP.Presentation.DTOs.Discount;
public class CreateDiscountRequest
{
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
