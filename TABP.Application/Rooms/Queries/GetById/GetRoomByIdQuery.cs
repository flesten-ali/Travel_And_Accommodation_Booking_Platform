using MediatR;
using TABP.Application.Rooms.Common;

namespace TABP.Application.Rooms.Queries.GetById;
public sealed record GetRoomByIdQuery(Guid RoomId, Guid RoomClassId) : IRequest<RoomResponse>;
