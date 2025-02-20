using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.RoomClasses.Commands.ImageGallery;
public class UploadRoomClassImageGalleryCommand : IRequest
{
    public Guid RoomClassId { get; set; }
    public IFormFile Image { get; set; }
}
