namespace TABP.Domain.Models;

public  class RecentlyVisitedHotelsResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Rate { get; set; }
    public string CityName { get; set; }
    public string ThumbnailUrl { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public DateTime BookingDate { get; set; }
    public Guid BookingId { get; set; }
    public double Price { get; set; }
}