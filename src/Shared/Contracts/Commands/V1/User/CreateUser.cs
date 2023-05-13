using Contracts.Common;

namespace Contracts.Commands.V1.User;

public record CreateUser : ICommand
{
  public required string UserName { get; init; }
  public required string ExternalAccount { get; init; }
  public required string ExternalAccountId { get; init; }
  public required string EmailAddress { get; init; }
  public DateTime BirthDate { get; init; }
  public string AvatarCode { get; init; }
  public Guid CorrelationId { get; set; }
}