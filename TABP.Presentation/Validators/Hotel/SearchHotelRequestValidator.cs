﻿using FluentValidation;
using TABP.Presentation.DTOs.Hotel;

namespace TABP.Presentation.Validators.Hotel;
public class SearchHotelRequestValidator : AbstractValidator<SearchHotelRequest>
{
    public SearchHotelRequestValidator()
    {
        RuleFor(h => h.City).NotEmpty().WithMessage("please enter a destination to start searching");
    }
}
