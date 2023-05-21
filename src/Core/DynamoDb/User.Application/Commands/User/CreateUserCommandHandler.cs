using System.Text.Json;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using User.Application.ViewModels;
using User.Domain.Entities;
using User.Domain.Extensions;
using User.Domain.Repositories;

namespace User.Application.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUser, bool>
{
  private readonly ILogger<CreateUserCommandHandler> _logger;
  private readonly IUserRepository _userRepository;

  public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUserRepository userRepository)
  {
    _logger = logger;
    _userRepository = userRepository;
  }

  public async Task<bool> Handle(CreateUser request, CancellationToken cancellationToken)
  {
    var existingUser =
      await _userRepository.GetAsync(request.ExternalAccount, request.ExternalAccountId, request.EmailAddress);
    if (existingUser is not null)
    {
      var message = $"A user with email {request.EmailAddress} already exists";
      throw new ValidationException(message, ValidationExtensions.GenerateValidationError(nameof(CreateUser), message));
    }

    var user = request.Adapt<UserEntity>();
    var result = await _userRepository.CreateAsync(user);
    if (result)
    {
      var viewModel = user.Adapt<UserViewModel>();
      _logger.LogInformation($"User create successfuly :{JsonSerializer.Serialize(viewModel)}");
    }

    return result;
  }
}