namespace TABP.Presentation.DTOs.Review;
public class GetReviewDetailsRequest
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
