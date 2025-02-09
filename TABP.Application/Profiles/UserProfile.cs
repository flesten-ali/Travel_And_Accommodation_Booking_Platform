using AutoMapper;
using TABP.Application.Users.Login;
using TABP.Application.Users.Register;
using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Application.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<JwtToken, LoginUserResponse>();
    }
}
