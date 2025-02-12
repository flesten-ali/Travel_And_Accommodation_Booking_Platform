using AutoMapper;
using TABP.Application.Cities.Queries.GetForAdmin;
using TABP.Application.Cities.Queries.GetTrending;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<TrendingCitiesResult, TrendingCitiesResponse>();
        CreateMap<CityForAdminResult, CityForAdminResponse>();
        CreateMap<PaginatedList<CityForAdminResult>, PaginatedList<CityForAdminResponse>>();
    }
}
