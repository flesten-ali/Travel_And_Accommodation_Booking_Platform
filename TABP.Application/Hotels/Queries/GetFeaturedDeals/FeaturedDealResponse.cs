namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;
public record FeaturedDealResponse(
    Guid Id,
    string Name,
    string? Description, 
    int Rate,
    string CityName, 
    string ThumbnailUrl,
    double OriginalPrice, 
    double DiscountedPrice);
