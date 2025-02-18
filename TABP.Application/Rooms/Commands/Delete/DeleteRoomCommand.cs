using MediatR;

namespace TABP.Application.Rooms.Commands.Delete;
public class DeleteRoomCommand : IRequest
{
    public Guid Id { get; set; }
}
