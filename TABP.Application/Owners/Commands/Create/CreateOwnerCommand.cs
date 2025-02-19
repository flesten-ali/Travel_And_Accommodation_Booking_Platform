using MediatR;
using TABP.Application.Owners.Common;

namespace TABP.Application.Owners.Commands.Create;
public class CreateOwnerCommand : IRequest<OwnerResponse>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}