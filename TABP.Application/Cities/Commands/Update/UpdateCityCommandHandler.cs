using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionsMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Update;
public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCityCommand request, CancellationToken cancellationToken = default)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(CityExceptionMessages.NotFound);

        _mapper.Map(request, city);

        _cityRepository.Update(city);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
