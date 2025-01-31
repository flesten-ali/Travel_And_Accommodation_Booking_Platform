using MediatR;

namespace TABP.Application.Users.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponse>;
