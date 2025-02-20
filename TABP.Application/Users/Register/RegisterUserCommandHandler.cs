using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Security.Password;
namespace TABP.Application.Users.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHash,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHash;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistsAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new ExistsException(UserExceptionMessages.Exist);
        }

        var user = _mapper.Map<User>(request);
        user.PasswordHash = _passwordHasher.Hash(request.Password);

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
