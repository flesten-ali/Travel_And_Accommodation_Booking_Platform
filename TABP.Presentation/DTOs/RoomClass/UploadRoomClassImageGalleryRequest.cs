using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.DTOs.RoomClass;
public class UploadRoomClassImageGalleryRequest
{
    public IFormFile Image { get; set; }
}
