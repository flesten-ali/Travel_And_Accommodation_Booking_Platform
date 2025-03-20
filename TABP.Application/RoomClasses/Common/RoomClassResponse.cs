using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Common;
public sealed record RoomClassResponse(
    Guid Id, 
    string Name,
    string? Description, 
    RoomType RoomType, 
    int AdultsCapacity, 
    int ChildrenCapacity,
    double Price);
