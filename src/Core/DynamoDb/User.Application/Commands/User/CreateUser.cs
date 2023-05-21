using MediatR;

namespace User.Application.Commands.User;

public record CreateUser : IRequest<bool>
{
  public required string UserName { get; init; }
  public required string ExternalAccount { get; init; }
  public required string ExternalAccountId { get; init; }
  public required string EmailAddress { get; init; }
  public DateTime BirthDate { get; init; }
  public string AvatarCode { get; init; }
}