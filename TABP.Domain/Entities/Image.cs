using TABP.Domain.Enums;
namespace TABP.Domain.Entities;

public class Image : EntityBase<Guid>
{
    public string ImageUrl { get; set; }
    public ImageableType ImageableType { get; set; }
    public Guid ImageableId { get; set; }
}
