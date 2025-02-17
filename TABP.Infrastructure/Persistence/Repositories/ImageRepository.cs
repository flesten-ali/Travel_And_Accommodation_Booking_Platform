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

    public async Task DeleteByIdAsync(Guid entityId, ImageType imageType, CancellationToken cancellationToken = default)
    {
        var images = await DbSet
                .Where(img =>
                        img.ImageableId == entityId &&
                        img.ImageType == imageType
                )
                .ToListAsync(cancellationToken);

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
