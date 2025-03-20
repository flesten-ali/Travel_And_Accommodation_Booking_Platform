using MediatR;

namespace TABP.Application.Rooms.Commands.Delete;
public sealed record DeleteRoomCommand(Guid RoomId, Guid RoomClassId) : IRequest;
