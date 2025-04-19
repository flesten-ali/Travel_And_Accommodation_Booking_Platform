using MediatR;
using Microsoft.AspNetCore.Http;

namespace TABP.Application.Cities.Commands.Thumbnail;
public class UploadCityThumbnailCommand : IRequest
{
    public Guid CityId { get; set; }
    public IFormFile Thumbnail { get; set; }
}