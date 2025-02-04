using TABP.Domain.Entities;
namespace TABP.Domain.Models;

public class SearchHotelResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Image? Thumbnail { get; set; }
    public int StarRating { get; set; }
    public double PricePerNight { get; set; }
    public string? Description { get; set; }
}
