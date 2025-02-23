namespace TABP.Application.Hotels.Queries.SearchHotels;

public sealed record SearchHotelResponse(
    Guid Id, 
    string Name,
    string? ThumbnailUrl,
    int StarRating, 
    double PricePerNight, 
    string? Description);
