using MediatR;
namespace TABP.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponse>;
