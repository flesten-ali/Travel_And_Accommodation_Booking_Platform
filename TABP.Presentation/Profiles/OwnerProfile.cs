using AutoMapper;
using TABP.Application.Owners.Commands.Create;
using TABP.Presentation.DTOs.Owner;

namespace TABP.Presentation.Profiles;
public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        CreateMap<CreateOwnerRequest, CreateOwnerCommand>();
    }
}
