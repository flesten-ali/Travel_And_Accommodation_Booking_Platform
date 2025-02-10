namespace TABP.Domain.Entities;

public class City : EntityBase<Guid>
{
    public string Name { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public ICollection<Hotel> Hotels { get; set; } = [];
    public Image Thumbnail { get; set; }
    public  Guid ThumbnailId { get; set; } 
}
