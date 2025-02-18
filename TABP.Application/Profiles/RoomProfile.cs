using AutoMapper;
using TABP.Application.Rooms.Commands.Create;
using TABP.Application.Rooms.Common;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<PaginatedList<RoomForAdminResult>, PaginatedList<RoomForAdminResponse>>();

        CreateMap<RoomForAdminResult, RoomForAdminResponse>();

        CreateMap<CreateRoomCommand, Room>();

        CreateMap<Room, RoomResponse>();
    }
}
