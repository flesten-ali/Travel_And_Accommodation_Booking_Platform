using AutoMapper;
using MediatR;
using TABP.Application.Hotels.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetById;

/// <summary>
/// Handles the retrieval of a hotel’s details by its ID, including the hotel’s associated owner and city information.
/// </summary>
public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelByIdQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to get a hotel’s details by its ID. This includes the hotel’s owner and city details.
    /// </summary>
    /// <param name="request">The query containing the hotel ID to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a <see cref="HotelResponse"/> containing the hotel details.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the hotel with the specified ID is not found in the repository.
    /// </exception>
    public async Task<HotelResponse> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdIncludePropertiesAsync(
            request.HotelId,
            cancellationToken,
            h => h.Owner,
            h => h.City)
            ?? throw new NotFoundException(HotelExceptionMessages.NotFound);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
