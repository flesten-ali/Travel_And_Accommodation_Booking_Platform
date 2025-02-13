using FluentValidation;
using TABP.Presentation.DTOs.City;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.City;
public class UploadCityThumbnailRequestValidator : AbstractValidator<UploadCityThumbnailRequest>
{
    public UploadCityThumbnailRequestValidator()
    {
        RuleFor(x => x.Thumbnail)
        .NotNull().WithMessage("Thumbnail file is required.")
        .SetValidator(new ImageFileValidator());
    }
}