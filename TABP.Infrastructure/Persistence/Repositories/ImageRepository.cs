using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Image;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class ImageRepository : Repository<Image>, IImageRepository
{
    private readonly IImageUploadService _imageUploadService;

    public ImageRepository(AppDbContext context, IImageUploadService imageUploadService) : base(context)
    {
        _imageUploadService = imageUploadService;
    }

    /// <summary>
    /// Deletes images associated with a specific entity by its ID and image type.
    /// </summary>
    /// <param name="entityId">The ID of the entity associated with the images.</param>
    /// <param name="imageType">The type of the images (e.g., Thumbnail, Gallery, etc.).</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    public async Task DeleteByIdAsync(Guid entityId, ImageType imageType, CancellationToken cancellationToken = default)
    {
        var images = await DbSet
                .Where(img =>
                        img.ImageableId == entityId &&
                        img.ImageType == imageType
                )
                .ToListAsync(cancellationToken);

        // If images are found, delete them from external storage and remove from the database
        if (images.Count != 0)
        {
            foreach (var image in images)
            {
                await _imageUploadService.DeleteAsync(image.PublicId);
                DbSet.Remove(image);
            }
        }
    }
}
