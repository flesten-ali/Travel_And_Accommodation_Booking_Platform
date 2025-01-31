using FluentValidation;
using TABP.Presentation.DTOs.Auth;
namespace TABP.Presentation.Validators.Auth;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email address is required!");

        RuleFor(u => u.UserName)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("password is required!")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                    .Matches(@"[\@\!\?\*\$]").WithMessage("Password must contain at least one special character (@, !, ?, *, $).");
    }
}

