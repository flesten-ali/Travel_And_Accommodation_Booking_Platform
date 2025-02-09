﻿namespace TABP.Application.Hotels.Queries.GetFeaturedDeals;
public class FeaturedDealResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int StarRate { get; set; }
    public string CityName { get; set; }
    public string ThumbnailUrl { get; set; }
    public double OriginalPrice { get; set; }
    public double DiscountedPrice { get; set; }
}
