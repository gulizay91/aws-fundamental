using MediatR;
using User.Application.ViewModels;

namespace User.Application.Queries.User;

public record GetUserByUserName : IRequest<UserViewModel?>
{
  public required string UserName { get; init; }
}