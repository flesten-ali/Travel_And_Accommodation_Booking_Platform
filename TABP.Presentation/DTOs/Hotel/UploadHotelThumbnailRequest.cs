using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs;

public class UploadHotelThumbnailRequest
{
    public IFormFile Thumbnail { get; set; }
}