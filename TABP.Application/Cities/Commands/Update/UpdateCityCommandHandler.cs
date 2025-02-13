using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Cities.Commands.Update;
public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCityCommandHandler(ICityRepository cityRepository , IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException("City not found");

        city.Name = request.Name;
        city.Country = request.Country;
        city.PostOffice = request.PostOffice;
        city.Address = request.Address;
        city.PostalCode = request.PostalCode;

        _cityRepository.Update(city);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
