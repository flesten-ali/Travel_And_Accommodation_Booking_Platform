using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Delete;
public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("City not found");

        _cityRepository.Delete(city);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
