using FluentValidation;

namespace User.Application.Commands.User;

public class CreateUserValidator : AbstractValidator<CreateUser>
{
  public CreateUserValidator()
  {
    RuleFor(t => t.UserName).NotNull().NotEmpty();
    RuleFor(t => t.ExternalAccount).NotNull().NotEmpty();
    RuleFor(t => t.ExternalAccountId).NotNull().NotEmpty();
    RuleFor(t => t.EmailAddress).NotNull().NotEmpty().EmailAddress();
    RuleFor(x => x.BirthDate).LessThan(DateTime.Now).WithMessage("Your date of birth cannot be in the future");
  }
}