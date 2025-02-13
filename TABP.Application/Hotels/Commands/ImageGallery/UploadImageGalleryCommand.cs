﻿using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Hotels.Commands.ImageGallery;
public class UploadImageGalleryCommand : IRequest
{
    public Guid HotelId { get; set; }
    public IFormFile Image { get; set; }
}
