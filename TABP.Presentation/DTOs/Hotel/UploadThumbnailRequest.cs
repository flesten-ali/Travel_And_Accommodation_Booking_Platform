using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs;

public class UploadThumbnailRequest
{
    public IFormFile Thumbnail { get; set; }
}