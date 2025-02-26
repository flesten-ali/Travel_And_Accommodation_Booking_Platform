namespace TABP.Domain.Models;

public sealed class SearchHotelResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int Rate { get; set; }
    public double PricePerNight { get; set; }
    public string? Description { get; set; }
}