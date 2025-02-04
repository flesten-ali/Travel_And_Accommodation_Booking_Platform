using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;
public class ImageRepository : Repository<Image>, IImageRepository
{
    private readonly IImageStorageService _imageStorageService;

    public ImageRepository(AppDbContext context, IImageStorageService imageStorageService) : base(context)
    {
        _imageStorageService = imageStorageService;
    }

    public async Task DeleteByIdAsync(Guid enittyId, ImageType imageType)
    {
        var images = await DbSet
                .Where(img =>
                        img.ImageableId == enittyId &&
                        img.ImageType == imageType
                )
                .ToListAsync();

        if (images.Count != 0)
        {
            foreach (var image in images)
            {
                await _imageStorageService.DeleteFileAsync(image.ImageUrl);
                DbSet.Remove(image);
            }
        }
    }
}
