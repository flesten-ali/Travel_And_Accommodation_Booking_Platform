using TABP.Domain.Common;
using TABP.Domain.Enums;
namespace TABP.Domain.Entities;

public class Image : EntityBase<Guid>
{
    public string ImageUrl { get; set; }
    public Guid ImageableId { get; set; }
    public ImageType ImageType { get; set; }
    public string PublicId {  get; set; }
}
