using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Common;
public class RoomClassResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public double Price { get; set; }
}