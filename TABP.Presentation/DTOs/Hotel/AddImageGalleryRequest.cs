using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs.Hotel;
public class AddImageGalleryRequest
{
    public Guid HotelId { get; set; }
    public IFormFile Image { get; set; }
}
