using AutoMapper;
using TABP.Application.Amenities.Commands.Create;
using TABP.Application.Amenities.Commands.Update;
using TABP.Presentation.DTOs.Amenity;
namespace TABP.Presentation.Profiles;

public class AmenityProfile : Profile
{
    public AmenityProfile()
    {

        CreateMap<CreateAmenityRequest, CreateAmenityCommand>();

        CreateMap<UpdateAmenityRequest, UpdateAmenityCommand>();
    }
}
