using MediatR;

namespace TABP.Application.Discounts.Commands.Delete;
public sealed record DeleteDiscountCommand(Guid DiscountId, Guid RoomClassId) : IRequest;
