using MediatR;

namespace TABP.Application.RoomClasses.Commands.Delete;
public sealed record DeleteRoomClassCommand(Guid Id) : IRequest;
