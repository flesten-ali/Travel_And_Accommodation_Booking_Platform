namespace TABP.Application.Hotels.Queries.SearchHotels;

public sealed record SearchHotelResponse(
    Guid Id, 
    string Name,
    string? ThumbnailUrl,
    int Rate, 
    double PricePerNight, 
    string? Description);
