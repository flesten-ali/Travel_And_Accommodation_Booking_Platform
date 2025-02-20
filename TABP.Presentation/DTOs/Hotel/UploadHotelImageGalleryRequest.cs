using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs.Hotel;
public class UploadHotelImageGalleryRequest
{
    public IFormFile Image { get; set; }
}
