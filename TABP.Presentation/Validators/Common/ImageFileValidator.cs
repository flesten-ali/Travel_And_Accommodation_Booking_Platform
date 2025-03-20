using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TABP.Presentation.Validators.Common;
public class ImageFileValidator : AbstractValidator<IFormFile>
{
    private static readonly string[] ValidFormats = ["image/jpeg", "image/png", "image/webp"];

    public ImageFileValidator()
    {
        RuleFor(file => file.Length)
          .GreaterThan(0).WithMessage("File cannot be empty.")
          .LessThanOrEqualTo(2 * 1024 * 1024).WithMessage("File size must not exceed 2MB.");

        RuleFor(file => file.ContentType)
            .Must(contentType => ValidFormats.Contains(contentType))
            .WithMessage("Only JPEG, PNG, and WEBP formats are allowed.");
    }
}
