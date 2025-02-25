using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using TABP.Domain.Interfaces.Services.Image;
using TABP.Infrastructure.Exceptions;

namespace TABP.Infrastructure.Services.Image;

/// <summary>
/// Service for uploading and deleting images using Cloudinary.
/// </summary>
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

    /// <summary>
    /// Uploads an image to Cloudinary asynchronously.
    /// </summary>
    /// <param name="file">The image file to upload.</param>
    /// <param name="publicId">The public identifier for the uploaded image.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>The secure URL of the uploaded image.</returns>
    /// <exception cref="Exception">Thrown if an error occurs during the upload process.</exception>
    public async Task<string> UploadAsync(IFormFile file, string publicId, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        var uploadparams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            PublicId = publicId,
        };

        var result = await _cloudinary.UploadAsync(uploadparams, cancellationToken);

        if (result.Error != null)
        {
            throw new CloudinaryException(result.Error.Message);
        }

        return result.SecureUrl.ToString();
    }

    /// <summary>
    /// Deletes an image from Cloudinary asynchronously.
    /// </summary>
    /// <param name="publicId">The public identifier of the image to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="Exception">Thrown if an error occurs during deletion.</exception>
    public async Task DeleteAsync(string publicId)
    {
        var destroyParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(destroyParams);

        if (result.Error != null)
        {
            throw new CloudinaryException(result.Error.Message);
        }
    }
}
