using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs;

public class AddThumbnailRequest
{
    public Guid HotelId { get; set; }
    public IFormFile Thumbnail { get; set; }
}
