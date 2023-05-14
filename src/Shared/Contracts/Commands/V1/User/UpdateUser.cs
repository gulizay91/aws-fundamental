using Contracts.Common;

namespace Contracts.Commands.V1.User;

public class UpdateUser : ICommand
{
  public required string Id { get; init; }
  public required string AvatarCode { get; init; }
  public Guid CorrelationId { get; set; }
}