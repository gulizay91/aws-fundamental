using Contracts.Commands.V1.User;
using MediatR;

namespace User.Consumer.Handlers.CommandHandler;

public class UpdateUserHandler : IRequestHandler<UpdateUser>
{
  private readonly ILogger<UpdateUserHandler> _logger;

  public UpdateUserHandler(ILogger<UpdateUserHandler> logger)
  {
    _logger = logger;
  }

  public async Task Handle(UpdateUser request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("ReceivedMessage: {0} for userId: {1} - CorrelationId: {2}", nameof(UpdateUser),
      request.Id, request.CorrelationId);
    await Task.CompletedTask;
  }
}