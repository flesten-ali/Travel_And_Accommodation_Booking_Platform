using MediatR;
namespace TABP.Application.Users.Register;

public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}