namespace TABP.Application.Hotels.Common;
public class HotelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Rate { get; set; }
    public double LongitudeCoordinates { get; set; }
    public double LatitudeCoordinates { get; set; }
    public string CityName { get; set; }
    public string OwnerName { get; set; }
    public string ThumbnailUrl { get; set; }
}