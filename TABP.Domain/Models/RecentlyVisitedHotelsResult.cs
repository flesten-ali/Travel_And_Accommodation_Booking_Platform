namespace TABP.Domain.Models;

public sealed record RecentlyVisitedHotelsResult(
    Guid Id,
    string Name,
    int Rate, 
    string CityName, 
    string ThumbnailUrl,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    DateTime BookingDate,
    Guid BookingId, 
    double Price);
