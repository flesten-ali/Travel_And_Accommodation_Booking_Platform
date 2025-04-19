using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs.City;
public class UploadCityThumbnailRequest
{
    public IFormFile Thumbnail { get; set; }
}
