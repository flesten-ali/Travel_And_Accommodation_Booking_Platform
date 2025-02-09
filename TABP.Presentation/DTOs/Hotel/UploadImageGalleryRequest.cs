using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs.Hotel;
public class UploadImageGalleryRequest
{
    public IFormFile Image { get; set; }
}
