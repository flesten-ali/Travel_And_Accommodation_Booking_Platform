namespace TABP.Domain.Models;

public sealed record FeaturedDealResult(
    Guid Id, 
    string Name,
    string? Description,
    int Rate,
    string CityName, 
    string ThumbnailUrl,
    double OriginalPrice, 
    double DiscountedPrice);
