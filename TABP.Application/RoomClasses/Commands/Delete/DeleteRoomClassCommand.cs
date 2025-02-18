using MediatR;

namespace TABP.Application.RoomClasses.Commands.Delete;
public class DeleteRoomClassCommand :IRequest
{
    public Guid Id { get; set; }
}