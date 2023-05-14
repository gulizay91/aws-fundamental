using Contracts.Commands.V1.User;
using MediatR;

namespace User.Consumer.Handlers.CommandHandler;

public class CreateUserHandler : IRequestHandler<CreateUser>
{
  private readonly ILogger<CreateUserHandler> _logger;

  public CreateUserHandler(ILogger<CreateUserHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(CreateUser request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("ReceivedMessage: {0} for username: {1} - CorrelationId: {2}", nameof(CreateUser),
      request.UserName, request.CorrelationId);
    await Task.CompletedTask;
  }
}