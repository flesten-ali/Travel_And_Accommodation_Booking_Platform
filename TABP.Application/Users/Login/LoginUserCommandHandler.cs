using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Security.Jwt;
using TABP.Domain.Interfaces.Security.Password;
namespace TABP.Application.Users.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtGenerator jwtGenerator,
        IMapper mapper
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.AuthenticateUserAsync(request.Email, request.Password)
                  ?? throw new UserUnauthorizedException("Invalid email or password");
        var token = _jwtGenerator.GenerateToken(user);
        return _mapper.Map<LoginUserResponse>(token);
    }
}
