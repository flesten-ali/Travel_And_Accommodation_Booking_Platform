using AutoMapper;
using TABP.Application.Cities.Commands.Create;
using TABP.Application.Cities.Commands.Thumbnail;
using TABP.Application.Cities.Common;
using TABP.Application.Cities.Queries.GetForAdmin;
using TABP.Application.Cities.Queries.GetTrending;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<TrendingCitiesResult, TrendingCitiesResponse>();

        CreateMap<CityForAdminResult, CityForAdminResponse>();

        CreateMap<PaginatedList<CityForAdminResult>, PaginatedList<CityForAdminResponse>>();

        CreateMap<CreateCityCommand, City>();

        CreateMap<City, CityResponse>();

        CreateMap<UploadCityThumbnailCommand, Image>()
            .ForMember(dest => dest.ImageableId, opt => opt.MapFrom(src => src.CityId))
            .ForMember(dest => dest.ImageType, opt => opt.MapFrom(src => ImageType.Thumbnail));
    }
}