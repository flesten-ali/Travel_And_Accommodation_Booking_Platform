using AutoMapper;
using TABP.Application.RoomClasses.Queries.GetDetails;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Profiles;
public class RoomClassProfile : Profile
{
    public RoomClassProfile()
    {
        CreateMap<GetHotelRoomClassesRequest, GetHotelRoomClassesQuery>();
    }
}
