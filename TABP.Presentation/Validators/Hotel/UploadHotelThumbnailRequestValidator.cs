using FluentValidation;
using TABP.Presentation.DTOs;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.Hotel;
public class UploadHotelThumbnailRequestValidator : AbstractValidator<UploadHotelThumbnailRequest>
{
    public UploadHotelThumbnailRequestValidator()
    {
        RuleFor(x => x.Thumbnail)
            .NotNull().WithMessage("Thumbnail file is required.")
            .SetValidator(new ImageFileValidator());
    }
}
