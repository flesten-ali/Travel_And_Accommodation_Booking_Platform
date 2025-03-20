using MediatR;
using TABP.Application.Owners.Common;

namespace TABP.Application.Owners.Commands.Create;
public sealed record CreateOwnerCommand(string Name, string Address, string Email, string Phone)
    : IRequest<OwnerResponse>;
