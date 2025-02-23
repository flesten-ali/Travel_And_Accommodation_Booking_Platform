using MediatR;
using TABP.Application.Hotels.Common;
namespace TABP.Application.Hotels.Queries.GetById;

public sealed record GetHotelByIdQuery(Guid HotelId) : IRequest<HotelResponse>;
