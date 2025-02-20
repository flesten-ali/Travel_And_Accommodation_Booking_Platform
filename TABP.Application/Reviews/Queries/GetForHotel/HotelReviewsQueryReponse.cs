namespace TABP.Application.Reviews.Queries.GetForHotel;
public class HotelReviewsQueryReponse
{
    public string Comment { get; set; }
    public int Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ReviwerName { get; set; }
}