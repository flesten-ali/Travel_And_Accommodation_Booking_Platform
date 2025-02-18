using MediatR;
using TABP.Application.RoomClasses.Common;
namespace TABP.Application.RoomClasses.Queries.GetById;

public class GetRoomClassByIdQuery : IRequest<RoomClassResponse>
{
    public Guid Id { get; set; }
}