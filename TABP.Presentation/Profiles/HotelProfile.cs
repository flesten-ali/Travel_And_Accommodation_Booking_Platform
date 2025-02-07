using AutoMapper;
using TABP.Application.Hotels.Queries.Search;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<SearchHotelRequest, SearchHotelQuery>();
    }
}
