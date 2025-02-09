using FluentValidation;
using Microsoft.AspNetCore.Http;
using TABP.Presentation.DTOs.Hotel;
using TABP.Presentation.Validators.Hotel.Common;

namespace TABP.Presentation.Validators.Hotel;
public class UploadImageGalleryRequestValidator : AbstractValidator<UploadImageGalleryRequest>
{
    public UploadImageGalleryRequestValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("HotelId is required.");

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Thumbnail file is required.")
           .SetValidator(new ImageFileValidator());
    }
}