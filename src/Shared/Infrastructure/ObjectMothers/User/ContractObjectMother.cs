using Bogus;
using Contracts.Commands.V1.User;

namespace Infrastructure.ObjectMothers.User;

public static class ContractObjectMother
{
  public static CreateUser SimpleFakeCreateUser()
  {
    var newUser = new Faker<CreateUser>()
      .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()))
      .RuleFor(u => u.EmailAddress, (f, u) => f.Internet.Email(f.Name.FirstName(), f.Name.LastName()))
      .RuleFor(u => u.AvatarCode, f => f.Internet.Avatar())
      .RuleFor(u => u.ExternalAccount, f => f.Company.CompanyName())
      .RuleFor(u => u.ExternalAccountId, f => f.UniqueIndex.ToString())
      .RuleFor(u => u.BirthDate, f => f.Date.Past())
      .RuleFor(u => u.CorrelationId, f => f.Random.Guid());

    return newUser.Generate();
  }
}