using Microsoft.AspNetCore.Http;

namespace TABP.Domain.Interfaces.Services.Image;
public interface IImageUploadService
{
    Task<string> UploadAsync(IFormFile file, string publicId);
    Task DeleteAsync(string publicId);
}