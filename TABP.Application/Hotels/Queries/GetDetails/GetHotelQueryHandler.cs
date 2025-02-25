using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetDetails;

/// <summary>
/// Handles the retrieval of a hotel’s detailed information by its ID.
/// </summary>
public class GetHotelQueryHandler : IRequestHandler<GetHotelQuery, HotelDetailsResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to retrieve detailed information for a hotel by its ID.
    /// </summary>
    /// <param name="request">The query containing the hotel ID to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result is a <see cref="HotelDetailsResponse"/> 
    /// containing the detailed hotel information.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown if the hotel with the specified ID is not found in the repository.</exception>
    public async Task<HotelDetailsResponse> Handle(GetHotelQuery request, CancellationToken cancellationToken = default)
    {
        var hotel = await _hotelRepository.GetByIdIncludePropertiesAsync(
            request.HotelId,
            cancellationToken)
            ?? throw new NotFoundException(HotelExceptionMessages.NotFound);

        return _mapper.Map<HotelDetailsResponse>(hotel);
    }
}
