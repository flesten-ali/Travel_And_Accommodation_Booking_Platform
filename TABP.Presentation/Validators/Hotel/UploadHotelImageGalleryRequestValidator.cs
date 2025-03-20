﻿using FluentValidation;
using TABP.Presentation.DTOs.Hotel;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.Hotel;
public class UploadHotelImageGalleryRequestValidator : AbstractValidator<UploadHotelImageGalleryRequest>
{
    public UploadHotelImageGalleryRequestValidator()
    {
        RuleFor(x => x.Image)
            .NotNull().WithMessage("Thumbnail file is required.")
           .SetValidator(new ImageFileValidator());
    }
}