using AutoMapper;
using TABP.Application.Amenities.Add;
using TABP.Presentation.DTOs.Amenity;

namespace TABP.Presentation.Profiles;
public class AmenityProfile : Profile
{
    public AmenityProfile()
    {

        CreateMap<AddAmenityRequest, AddAmenityCommand>();
    }
}
