using MediatR;
using TABP.Application.Rooms.Common;

namespace TABP.Application.Rooms.Queries.GetById;
public class GetRoomByIdQuery : IRequest<RoomResponse>
{
    public Guid RoomId { get; set; }
}
