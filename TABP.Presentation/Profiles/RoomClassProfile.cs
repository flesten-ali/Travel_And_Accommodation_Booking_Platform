using AutoMapper;
using TABP.Application.RoomClasses.Commands.Create;
using TABP.Application.RoomClasses.Queries.GetForAdmin;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Profiles;
public class RoomClassProfile : Profile
{
    public RoomClassProfile()
    {
        CreateMap<GetRoomClassesForAdminRequest, GetRoomClassesForAdminQuery>();

        CreateMap<CreateRoomClassRequest, CreateRoomClassCommand>();
    }
}
