using FluentValidation;
using Microsoft.AspNetCore.Http;
using TABP.Presentation.DTOs;

namespace TABP.Presentation.Validators.Hotel;
public class AddThumbnailRequestValidator : AbstractValidator<AddThumbnailRequest>
{
    public AddThumbnailRequestValidator()
    {
        RuleFor(x => x.HotelId)
             .NotEmpty().WithMessage("HotelId is required.");

        RuleFor(x => x.Thumbnail)
            .NotNull().WithMessage("Thumbnail file is required.")
            .Must(file => file.Length > 0).WithMessage("Thumbnail file cannot be empty.")
            .Must(file => file.Length <= 2 * 1024 * 1024).WithMessage("Thumbnail file size must not exceed 2MB.")
            .Must(file => IsValidImageFormat(file)).WithMessage("Only JPEG, PNG, and WEBP formats are allowed.");
    }

    private static bool IsValidImageFormat(IFormFile file)
    {
        var validFormates = new[] { "image/jpeg", "image/png", "image/webp" };
        return validFormates.Contains(file.ContentType);
    }
}
