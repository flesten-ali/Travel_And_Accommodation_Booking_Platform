using AutoMapper;
using TABP.Application.Users.Login;
using TABP.Application.Users.Register;
using TABP.Presentation.DTOs.Auth;
namespace TABP.Presentation.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, RegisterUserCommand>();
        CreateMap<RegisterAdminRequest, RegisterUserCommand>();
        CreateMap<LoginUserRequest, LoginUserCommand>();
    }
}
