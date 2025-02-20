namespace TABP.Application.Reviews.Common;
public class ReviewResponse
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public int Rate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid HotelId { get; set; }
}
