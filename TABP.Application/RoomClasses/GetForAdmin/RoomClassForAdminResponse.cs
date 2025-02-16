using System.Runtime;
using TABP.Domain.Entities;
using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.GetForAdmin;
public class RoomClassForAdminResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int NumberOfRooms { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
}