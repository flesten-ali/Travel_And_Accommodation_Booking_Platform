using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Queries.GetForAdmin;

public sealed record GetHotelsForAdminQuery(PaginationParameters PaginationParameters)
    : IRequest<PaginatedResponse<HotelForAdminResponse>>;
