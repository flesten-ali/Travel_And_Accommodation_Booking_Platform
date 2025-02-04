﻿using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Hotels.AddThumbnail;

public class AddThumbnailCommand : IRequest<Guid>
{
    public Guid HotelId { get; set; }
    public IFormFile Thumbnail { get; set; }
}
