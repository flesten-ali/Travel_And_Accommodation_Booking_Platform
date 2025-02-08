using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using TABP.Domain.Interfaces.Services.Image;

namespace TABP.Infrastructure.Services.Image;
public class CloudinaryImageUploadService : IImageUploadService
{

    private readonly Cloudinary _cloudinary;
    private readonly CloudinaryConfig _cloudinaryConfig;

    public CloudinaryImageUploadService(CloudinaryConfig cloudinaryConfig)
    {
        _cloudinaryConfig = cloudinaryConfig;

        var account = new Account(_cloudinaryConfig.Cloud, _cloudinaryConfig.ApiKey, _cloudinaryConfig.ApiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadAsync(IFormFile file, string publicId)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
       
        var uploadparams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            PublicId = publicId,
        };

        var result = await _cloudinary.UploadAsync(uploadparams);

        if (result.Error != null)
        {
            throw new Exception($"Cloudinary error occured: {result.Error.Message}");
        }

        return result.SecureUrl.ToString();
    }

    public async Task DeleteAsync(string publicId)
    {
        var destroyParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(destroyParams);

        if (result.Error != null)
        {
            throw new Exception($"Cloudinary error occured: {result.Error.Message}");
        }
    }
}
