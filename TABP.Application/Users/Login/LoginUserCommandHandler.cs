using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Security.Jwt;

namespace TABP.Application.Users.Login;

/// <summary>
/// Handles the login request for a user, including authentication and token generation.
/// </summary>
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IJwtGenerator jwtGenerator,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the login request by authenticating the user and generating a JWT token.
    /// </summary>
    /// <param name="request">The login command containing the user's email and password.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning the login response with a JWT token.</returns>
    /// <exception cref="UnauthorizedException">Thrown if the user authentication fails.</exception>
    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.AuthenticateUserAsync(
            request.Email,
            request.Password,
            cancellationToken)
            ?? throw new UnauthorizedException(UserExceptionMessages.UnauthorizedLogin);

        var token = _jwtGenerator.GenerateToken(user);

        return _mapper.Map<LoginUserResponse>(token);
    }
}
