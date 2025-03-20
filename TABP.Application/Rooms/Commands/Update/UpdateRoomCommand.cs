using MediatR;

namespace TABP.Application.Rooms.Commands.Update;
public class UpdateRoomCommand : IRequest
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public Guid RoomClassId { get; set; }
}
