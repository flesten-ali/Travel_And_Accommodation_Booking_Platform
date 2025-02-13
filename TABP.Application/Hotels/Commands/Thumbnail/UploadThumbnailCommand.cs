﻿using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Hotels.Commands.Thumbnail;

public class UploadThumbnailCommand : IRequest 
{
    public Guid HotelId { get; set; }
    public IFormFile Thumbnail { get; set; }
}
