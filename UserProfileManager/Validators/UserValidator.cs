using FluentValidation;
using UserProfileManager.Models;

public class UserValidator : AbstractValidator<User>
{
  public UserValidator()
  {
    RuleFor(x => x.UniqueName)
        .NotEmpty().WithMessage("UniqueName is required.")
        .MaximumLength(256).WithMessage("UniqueName must not exceed 256 characters.");

    RuleFor(x => x.Password)
        .NotEmpty().When(x => x.Id == 0).WithMessage("Password is required for new users.")
        .MaximumLength(256).WithMessage("Password must not exceed 256 characters.");

    RuleFor(x => x.FirstName)
        .NotEmpty().WithMessage("FirstName is required.")
        .MaximumLength(64).WithMessage("FirstName must not exceed 64 characters.");

    RuleFor(x => x.LastName)
        .NotEmpty().WithMessage("LastName is required.")
        .MaximumLength(64).WithMessage("LastName must not exceed 64 characters.");

    RuleFor(x => x.MiddleName)
        .MaximumLength(64).WithMessage("MiddleName must not exceed 64 characters.");

    RuleFor(x => x.Address)
        .MaximumLength(256).WithMessage("Address must not exceed 256 characters.");

    RuleFor(x => x.City)
        .MaximumLength(128).WithMessage("City must not exceed 128 characters.");

    RuleFor(x => x.CountryCode)
        .NotEmpty().WithMessage("CountryCode is required.")
        .MaximumLength(5).WithMessage("CountryCode must not exceed 5 characters.");

    RuleFor(x => x.PhoneNumber)
        .NotEmpty().WithMessage("PhoneNumber is required.")
        .MaximumLength(15).WithMessage("PhoneNumber must not exceed 15 characters.");

    RuleFor(x => x.Gender)
        .NotEmpty().WithMessage("Gender is required.")
        .Matches("^[MFO]$").WithMessage("Gender must be one of M, F, or O.");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email is not valid.")
        .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

    RuleFor(x => x.DateOfBirth)
        .NotEmpty().WithMessage("DateOfBirth is required.");

    RuleFor(x => x.Education)
        .MaximumLength(128).WithMessage("Education must not exceed 128 characters.");

    RuleFor(x => x.Photo)
        .Must(photo => photo == null || photo.Length <= 5 * 1024 * 1024).WithMessage("Photo must not exceed 5MB.");
  }
}
