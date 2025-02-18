using MediatR;
using TABP.Application.Rooms.Common;

namespace TABP.Application.Rooms.Commands.Create;

public class CreateRoomCommand : IRequest<RoomResponse>
{
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public Guid RoomClassId { get; set; }
}
