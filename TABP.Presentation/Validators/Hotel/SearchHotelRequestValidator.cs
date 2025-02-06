using FluentValidation;
using TABP.Presentation.DTOs.Hotel;

namespace TABP.Presentation.Validators.Hotel;
public class SearchHotelRequestValidator : AbstractValidator<SearchHotelRequest>
{
    public SearchHotelRequestValidator()
    {
        RuleFor(x => x.City)
           .NotEmpty().WithMessage("City is required.")
           .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.");

        RuleFor(x => x.CheckInDate)
            .NotNull().WithMessage("Check-in date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date cannot be in the past.")
            .LessThan(x => x.CheckOutDate)
            .WithMessage("Check-in date must be before the check-out date.");

        RuleFor(x => x.CheckOutDate)
            .NotNull().WithMessage("Check-out date is required.")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after the check-in date.");

        RuleFor(x => x.ChildrenCapacity)
            .GreaterThanOrEqualTo(0).WithMessage("Children capacity cannot be negative.");

        RuleFor(x => x.AdultsCapacity)
            .GreaterThan(0).WithMessage("Adults capacity must be at least 1.");

        RuleFor(x => x.NumberOfRooms)
            .GreaterThan(0).WithMessage("Number of rooms must be at least 1.");

        RuleFor(x => x.SortBy)
            .Must(value => string.IsNullOrEmpty(value) || new[] { "price", "starrating", "name" }.Contains(value.ToLower()))
            .WithMessage("SortBy must be 'price', 'starrating', or 'name'.");

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Min price cannot be negative.")
            .LessThanOrEqualTo(x => x.MaxPrice).When(x => x.MaxPrice.HasValue)
            .WithMessage("Min price cannot be greater than max price.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Max price cannot be negative.");

        RuleFor(x => x.StarRating)
            .InclusiveBetween(1, 5).When(x => x.StarRating.HasValue)
            .WithMessage("Star rating must be between 1 and 5.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}
