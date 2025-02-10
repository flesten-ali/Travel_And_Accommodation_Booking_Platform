namespace TABP.Domain.Entities;

public class Hotel : EntityBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Rate { get; set; }
    public double LongitudeCoordinates { get; set; }
    public double LatitudeCoordinates { get; set; }
    public City City { get; set; }
    public Guid CityId { get; set; }
    public Owner Owner { get; set; }
    public Guid OwnerId { get; set; }
    public ICollection<Image> Gallery { get; set; } = [];
    public Image Thumbnail { get; set; }
    public ICollection<RoomClass> RoomClasses { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
}