using MediatR;

namespace TABP.Application.Hotels.Commands.Delete;
public class DeleteHotelCommand : IRequest
{
    public Guid Id { get; set; }
}
