using Microsoft.AspNetCore.Http;

namespace TABP.Domain.Interfaces.Services.Image;
public interface IImageStorageService
{
    Task DeleteFileAsync(string path);
    Task<string> UploadFileAsync(IFormFile file);
}
