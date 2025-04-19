using FluentValidation;

namespace TABP.Infrastructure.Services.Email;

public class SMTPOptionsValidator : AbstractValidator<SMTPConfig>
{
    public SMTPOptionsValidator()
    {
        RuleFor(x => x.Host)
           .NotEmpty().WithMessage("SMTP Host is required.");

        RuleFor(x => x.Port)
            .GreaterThan(0).WithMessage("Port must be a positive number.")
            .LessThanOrEqualTo(65535).WithMessage("Port must be a valid port number.");

        RuleFor(x => x.From)
            .NotEmpty().WithMessage("Sender email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
