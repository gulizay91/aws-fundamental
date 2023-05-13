using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Contracts.Common;
using Infrastructure.Loggers;

namespace API.Services;

public class AmazonSqsService : IAmazonSqsService
{
  private readonly IAmazonSQS _amazonSqsClient;
  private readonly IConsoleLogger _logger;

  public AmazonSqsService(IAmazonSQS amazonSqsClient, IConsoleLogger logger)
  {
    _amazonSqsClient = amazonSqsClient;
    _logger = logger;
  }

  public async Task<string> GetQueueUrl(string queueName)
  {
    var queueUrlResponse = await _amazonSqsClient.GetQueueUrlAsync(queueName);
    return queueUrlResponse.QueueUrl;
  }

  public async Task<SendMessageResponse?> PublishStandartMessageAsync<TEvent>(TEvent message, string queueName,
    Dictionary<string, MessageAttributeValue>? messageAttributes = null, int? delaySeconds = null)
  {
    ArgumentNullException.ThrowIfNull(message);
    ArgumentException.ThrowIfNullOrEmpty(queueName);

    var sendMessageRequest = new SendMessageRequest
    {
      QueueUrl = await GetQueueUrl(queueName),
      MessageBody = JsonSerializer.Serialize(message)
    };

    if (messageAttributes is not null) sendMessageRequest.MessageAttributes = messageAttributes;
    if (delaySeconds is not null) sendMessageRequest.DelaySeconds = delaySeconds.Value;

    var sendResponse = await _amazonSqsClient.SendMessageAsync(sendMessageRequest);

    var contract = (IContract)message!;
    await _logger.LogInformation(
      $"TEvent:{message.GetType().Name}:queue:{queueName}:messageId:{sendResponse?.MessageId}",
      requestBody: JsonSerializer.Serialize(message),
      responseBody: JsonSerializer.Serialize(sendResponse?.MD5OfMessageBody),
      httpStatusCode: sendResponse?.HttpStatusCode,
      correlationId: contract.CorrelationId);
    return sendResponse;
  }
}