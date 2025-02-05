using Microsoft.AspNetCore.Http;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Services.Image;
namespace TABP.Infrastructure.Services.Image;

public class ImageStorageService : IImageStorageService
{
    private readonly string _folderPath;
    public ImageStorageService(string folderPath)
    {
        _folderPath = folderPath;

        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new IsEmptyException($"File is empty {nameof(file)}");
        }

        string fileExtension = Path.GetExtension(file.FileName);
        string fileName = $"{Guid.NewGuid()}{fileExtension}";

        string filePath = Path.Combine(_folderPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return filePath;
    }

    public async Task DeleteFileAsync(string path)
    {
        if (File.Exists(path))
        {
            await Task.Run(() => File.Delete(path));
        }
        else
        {
            throw new FileNotFoundException();
        }
    }
}
