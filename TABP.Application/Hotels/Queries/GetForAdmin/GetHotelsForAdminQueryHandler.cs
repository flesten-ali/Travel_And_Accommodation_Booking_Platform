using AutoMapper;
using MediatR;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Hotels.Queries.GetForAdmin;
public class GetHotelsForAdminQueryHandler
    : IRequestHandler<GetHotelsForAdminQuery, PaginatedList<HotelForAdminResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelsForAdminQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<HotelForAdminResponse>> Handle(GetHotelsForAdminQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetHotelsForAdmin(request.PageSize, request.PageNumber);

        return _mapper.Map<PaginatedList<HotelForAdminResponse>>(hotels);
    }
}