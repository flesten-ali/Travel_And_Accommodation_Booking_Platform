using MediatR;

namespace TABP.Application.Owners.Commands.Delete;
public sealed record DeleteOwnerCommand(Guid Id) : IRequest;
