using MediatR;
using TABP.Application.RoomClasses.Common;
namespace TABP.Application.RoomClasses.Queries.GetById;

public sealed record GetRoomClassByIdQuery(Guid Id) : IRequest<RoomClassResponse>;
