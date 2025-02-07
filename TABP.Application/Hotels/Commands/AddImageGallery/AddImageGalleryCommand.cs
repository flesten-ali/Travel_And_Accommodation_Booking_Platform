using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Hotels.Commands.AddImageGallery;
public class AddImageGalleryCommand : IRequest<Guid>
{
    public Guid HotelId { get; set; }
    public IFormFile Image { get; set; }
}
