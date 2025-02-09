using FluentValidation;
using TABP.Presentation.DTOs;
using TABP.Presentation.Validators.Hotel.Common;

namespace TABP.Presentation.Validators.Hotel;
public class UploadThumbnailRequestValidator : AbstractValidator<UploadThumbnailRequest>
{
    public UploadThumbnailRequestValidator()
    {
        RuleFor(x => x.Thumbnail)
            .NotNull().WithMessage("Thumbnail file is required.")
            .SetValidator(new ImageFileValidator());
    }
}
