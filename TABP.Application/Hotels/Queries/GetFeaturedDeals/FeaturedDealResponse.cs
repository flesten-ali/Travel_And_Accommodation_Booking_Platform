namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;
public record FeaturedDealResponse(
    Guid Id,
    string Name,
    string? Description, 
    int StarRate,
    string CityName, 
    string ThumbnailUrl,
    double OriginalPrice, 
    double DiscountedPrice);
