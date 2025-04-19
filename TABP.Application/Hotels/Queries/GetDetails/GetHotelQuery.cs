using MediatR;
namespace TABP.Application.Hotels.Queries.GetDetails;

public sealed record GetHotelQuery(Guid HotelId) : IRequest<HotelDetailsResponse>;
