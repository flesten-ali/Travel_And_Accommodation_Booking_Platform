namespace TABP.Application.Hotels.Queries.GetRecentlyVisited;
public sealed record RecentlyVisitedHotelsResponse(
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
