using MediatR;

namespace TABP.Application.Rooms.Commands.Delete;
public class DeleteRoomCommand : IRequest
{
    public Guid RoomId { get; set; }
    public Guid RoomClassId { get; set; }
}
