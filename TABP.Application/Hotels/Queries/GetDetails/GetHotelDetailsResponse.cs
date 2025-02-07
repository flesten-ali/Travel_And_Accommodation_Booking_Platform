namespace TABP.Application.Hotels.Queries.GetDetails;
public class GetHotelDetailsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string City { get; set; }
    public int Rate { get; set; }
    public double LongitudeCoordinates { get; set; }
    public double LatitudeCoordinates { get; set; }
    public IEnumerable<string> GalleryUrls { get; set; } = [];
}
