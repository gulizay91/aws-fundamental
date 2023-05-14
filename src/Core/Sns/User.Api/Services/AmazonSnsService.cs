using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Infrastructure.Constants;
using Infrastructure.Loggers;

namespace User.Api.Services;

public class AmazonSnsService : IAmazonSnsService
{
  private readonly IAmazonSimpleNotificationService _amazonSnsClient;
  private readonly IConsoleLogger _logger;

  public AmazonSnsService(IAmazonSimpleNotificationService amazonSnsClient, IConsoleLogger logger)
  {
    _amazonSnsClient = amazonSnsClient;
    _logger = logger;
  }

  public async ValueTask<string> GetTopicArn(string queueName)
  {
    var topicArnResponse = await _amazonSnsClient.FindTopicAsync(queueName);
    return topicArnResponse.TopicArn;
  }

  public async Task<PublishResponse?> PublishStandartMessageAsync<TEvent>(TEvent message, string queueName,
    Dictionary<string, MessageAttributeValue>? messageAttributes = null)
  {
    ArgumentNullException.ThrowIfNull(message);
    ArgumentException.ThrowIfNullOrEmpty(queueName);

    var publishMessageRequest = new PublishRequest
    {
      TopicArn = await GetTopicArn(queueName),
      Message = JsonSerializer.Serialize(message)
    };

    if (messageAttributes is not null) publishMessageRequest.MessageAttributes = messageAttributes;

    var sendResponse = await _amazonSnsClient.PublishAsync(publishMessageRequest);

    var correlationId = default(Guid?);
    if (publishMessageRequest.MessageAttributes.TryGetValue(ContractAttributes.CorrelationId,
          out var correlationAttribute))
      correlationId = Guid.Parse(correlationAttribute.StringValue);
    await _logger.LogInformation(
      $"TEvent:{message.GetType().Name}:queue:{queueName}:messageId:{sendResponse?.MessageId}",
      requestBody: JsonSerializer.Serialize(message),
      httpStatusCode: sendResponse?.HttpStatusCode,
      correlationId: correlationId);
    return sendResponse;
  }
}