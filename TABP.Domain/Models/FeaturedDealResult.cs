namespace TABP.Domain.Models;

public sealed class FeaturedDealResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Rate { get; set; }
    public string CityName { get; set; }
    public string ThumbnailUrl { get; set; }
    public double OriginalPrice { get; set; }
    public double DiscountedPrice { get; set; }
}