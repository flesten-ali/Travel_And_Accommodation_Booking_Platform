namespace TABP.Application.Hotels.Common;
public sealed record HotelResponse(
    Guid Id,
    string Name, 
    string? Description,
    int Rate,
    double LongitudeCoordinates, 
    double LatitudeCoordinates,
    string CityName,
    string OwnerName,
    string ThumbnailUrl);
