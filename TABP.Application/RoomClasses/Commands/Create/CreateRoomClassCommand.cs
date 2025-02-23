using MediatR;
using TABP.Application.RoomClasses.Common;
using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Commands.Create;
public record CreateRoomClassCommand(
    string Name, 
    string? Description,
    RoomType RoomType, 
    int AdultsCapacity,
    int ChildrenCapacity,
    double Price,
    Guid HotelId,
    IEnumerable<Guid> AmenityIds) : IRequest<RoomClassResponse>;
