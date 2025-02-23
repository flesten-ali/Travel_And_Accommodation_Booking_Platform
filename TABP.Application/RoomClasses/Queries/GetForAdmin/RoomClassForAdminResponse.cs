using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Queries.GetForAdmin;
public record RoomClassForAdminResponse(
    Guid Id,
    string Name,
    string? Description,
    RoomType RoomType,
    int NumberOfRooms,
    int AdultsCapacity,
    int ChildrenCapacity);
