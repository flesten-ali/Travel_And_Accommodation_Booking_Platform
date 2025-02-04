using Microsoft.AspNetCore.Http;

namespace TABP.Domain.Interfaces.Services;
public interface IImageStorageService
{
    Task DeleteFileAsync(string path);
    Task<string> UploadFileAsync(IFormFile file);
}
