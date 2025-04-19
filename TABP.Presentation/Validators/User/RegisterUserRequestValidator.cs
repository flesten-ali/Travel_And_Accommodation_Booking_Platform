using FluentValidation;
using TABP.Presentation.DTOs.User;
namespace TABP.Presentation.Validators.User;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.UserName)
             .NotEmpty().WithMessage("User name is required.")
             .Length(3, 50).WithMessage("User name must be between 3 and 50 characters.");

        RuleFor(x => x.Password)
              .NotEmpty().WithMessage("Password is required.")
              .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
              .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
              .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
              .Matches(@"\d").WithMessage("Password must contain at least one digit.")
              .Matches(@"[\@\!\?\*\$]").WithMessage("Password must contain at least one special character (@, !, ?, *, $).");
    }
}

