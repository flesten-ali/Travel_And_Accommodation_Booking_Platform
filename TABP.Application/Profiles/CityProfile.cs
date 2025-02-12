using AutoMapper;
using TABP.Application.Cities.Commands.Create;
using TABP.Application.Cities.Common;
using TABP.Application.Cities.Queries.GetTrending;
using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<TrendingCitiesResult, TrendingCitiesResponse>();
        CreateMap<CityForAdminResult, CityResponse>();
        CreateMap<PaginatedList<CityForAdminResult>, PaginatedList<CityResponse>>();
        CreateMap<CreateCityCommand, City>();
        CreateMap<City, CityResponse>();
    }
}