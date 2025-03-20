using AutoMapper;
using TABP.Application.Rooms.Commands.Create;
using TABP.Application.Rooms.Commands.Update;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Presentation.DTOs.Room;

namespace TABP.Presentation.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<GetRoomsForAdminRequest, GetRoomsForAdminQuery>();

        CreateMap<CreateRoomRequest, CreateRoomCommand>();

        CreateMap<UpdateRoomRequest, UpdateRoomCommand>();
    }
}
