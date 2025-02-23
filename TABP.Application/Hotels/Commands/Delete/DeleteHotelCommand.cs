using MediatR;

namespace TABP.Application.Hotels.Commands.Delete;
public sealed record DeleteHotelCommand(Guid Id) : IRequest;
