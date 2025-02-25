using TABP.Domain.Enums;

namespace TABP.Domain.Models;

public sealed record RoomClassForAdminResult(
    Guid Id,
    string Name,
    string? Description, 
    RoomType RoomType, 
    int NumberOfRooms, 
    int AdultsCapacity, 
    int ChildrenCapacity);
