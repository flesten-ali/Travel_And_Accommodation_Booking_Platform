using AutoMapper;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Presentation.DTOs.Room;

namespace TABP.Presentation.Profiles;
public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<GetRoomsForAdminRequest, GetRoomsForAdminQuery>();
    }
}
