namespace TABP.Domain.Models;

public sealed record SearchHotelResult(
    Guid Id, 
    string Name, 
    string? ThumbnailUrl, 
    int Rate, 
    double PricePerNight,
    string? Description);
