using AutoMapper;
using TABP.Application.Rooms.Commands.Create;
using TABP.Application.Rooms.Commands.Update;
using TABP.Application.Rooms.Common;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<PaginatedResponse<RoomForAdminResult>, PaginatedResponse<RoomForAdminResponse>>();

        CreateMap<RoomForAdminResult, RoomForAdminResponse>();

        CreateMap<CreateRoomCommand, Room>();

        CreateMap<Room, RoomResponse>();

        CreateMap<UpdateRoomCommand, Room>();
    }
}
