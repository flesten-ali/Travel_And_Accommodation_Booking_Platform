using TABP.Domain.Enums;

namespace TABP.Domain.Models;

public sealed class RoomClassForAdminResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int NumberOfRooms { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
}