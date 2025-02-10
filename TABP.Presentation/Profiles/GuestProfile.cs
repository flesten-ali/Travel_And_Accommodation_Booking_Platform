using AutoMapper;
using TABP.Application.Hotels.Queries.GetRecentlyVisited;
using TABP.Presentation.DTOs.Guest;

namespace TABP.Presentation.Profiles;
public class GuestProfile : Profile
{
    public GuestProfile()
    {
        CreateMap<GetRecentlyVisitedHotelsRequest, GetRecentlyVisitedHotelsQuery>();
    }
}
