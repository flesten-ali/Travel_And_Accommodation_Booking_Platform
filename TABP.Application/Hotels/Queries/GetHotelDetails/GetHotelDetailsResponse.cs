using TABP.Domain.Entities;
using TABP.Domain.Enums;

namespace TABP.Application.Hotels.Queries.GetHotelDetails;
public class GetHotelDetailsResponse
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string City { get; set; }
    public int StarRating { get; set; }
    public double LongitudeCoordinates { get; set; }
    public double LatitudeCoordinates { get; set; }
    public IEnumerable<Image> Gallery { get; set; } = [];
}
 