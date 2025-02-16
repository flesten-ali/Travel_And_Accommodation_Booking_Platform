using AutoMapper;
using MediatR;
using TABP.Application.Exceptions;
using TABP.Domain.Entities;
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

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new ExistsException($"User with email {request.Email} is exits");
        }

        var user = _mapper.Map<User>(request);

        user.PasswordHash = _passwordHasher.Hash(request.Password);
        await _userRepository.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
