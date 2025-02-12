using AutoMapper;
using TABP.Application.Cities.GetForAdmin;
using TABP.Presentation.DTOs.City;

namespace TABP.Presentation.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<GetCitiesForAdminRequest, GetCitiesForAdminQuery>();
    }
}
