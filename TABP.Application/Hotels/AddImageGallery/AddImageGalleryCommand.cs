using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Hotels.AddImageGallery;
public class AddImageGalleryCommand : IRequest<Guid>
{
    public Guid HotelId { get; set; }
    public IFormFile Image { get; set; }
}
