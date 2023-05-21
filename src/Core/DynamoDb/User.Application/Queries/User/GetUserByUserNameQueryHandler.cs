using System.Text.Json;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using User.Application.ViewModels;
using User.Domain.Repositories;

namespace User.Application.Queries.User;

public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserName, UserViewModel?>
{
  private readonly ILogger<GetUserByUserNameQueryHandler> _logger;
  private readonly IUserRepository _userRepository;

  public GetUserByUserNameQueryHandler(ILogger<GetUserByUserNameQueryHandler> logger, IUserRepository userRepository)
  {
    _logger = logger;
    _userRepository = userRepository;
  }

  public async Task<UserViewModel?> Handle(GetUserByUserName request, CancellationToken cancellationToken)
  {
    var existingUser = await _userRepository.GetByUserNameAsync(request.UserName);
    if (existingUser is null)
    {
      _logger.LogInformation($"A user with username {request.UserName} not exists");
      return null;
    }

    var viewModel = existingUser.Adapt<UserViewModel>();
    _logger.LogInformation($"User create successfuly :{JsonSerializer.Serialize(viewModel)}");

    return viewModel;
  }
}