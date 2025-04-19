using AutoMapper;
using TABP.Application.Cities.Commands.Create;
using TABP.Application.Cities.Commands.Thumbnail;
using TABP.Application.Cities.Commands.Update;
using TABP.Application.Cities.Queries.GetForAdmin;
using TABP.Presentation.DTOs.City;

namespace TABP.Presentation.Profiles;
public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<GetCitiesForAdminRequest, GetCitiesForAdminQuery>();
        CreateMap<CreateCityRequest, CreateCityCommand>();
        CreateMap<UpdateCityRequest, UpdateCityCommand>();
        CreateMap<UploadCityThumbnailRequest, UploadCityThumbnailCommand>();
    }
}
