using FluentValidation;
using TABP.Presentation.DTOs.Hotel;

namespace TABP.Presentation.Validators.Hotels;
public class SearchHotelRequestValidator : AbstractValidator<SearchHotelRequest>
{
    public SearchHotelRequestValidator()
    {
        RuleFor(h => h.City).NotEmpty().WithMessage("please enter a destination to start searching");
    }
}
