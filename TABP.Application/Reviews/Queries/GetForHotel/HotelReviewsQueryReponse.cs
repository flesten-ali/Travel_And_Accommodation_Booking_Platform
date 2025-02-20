namespace TABP.Application.Reviews.Queries.GetForHotel;
public class HotelReviewsQueryReponse
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public int Rate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string ReviwerName { get; set; }
}