using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Contracts.Common;
using Infrastructure.Constants;
using Infrastructure.Loggers;
using MediatR;

namespace User.Consumer.Services.AmazonSqs;

public class AmazonSqsService : IAmazonSqsService
{
  private readonly IAmazonSQS _amazonSqsClient;
  private readonly IConsoleLogger _logger;
  private readonly IMediator _mediator;

  public AmazonSqsService(IAmazonSQS amazonSqsClient, IConsoleLogger logger, IMediator mediator)
  {
    _amazonSqsClient = amazonSqsClient;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task<string> GetQueueUrl(string queueName)
  {
    var queueUrlResponse = await _amazonSqsClient.GetQueueUrlAsync(queueName);
    return queueUrlResponse.QueueUrl;
  }

  public async Task<ReceiveMessageResponse?> SubscribeStandartMessageAsync(CancellationToken cancellationToken,
    string queueName,
    List<string>? attributeNames = null, List<string>? messageAttributeNames = null, int? maxNumberOfMessages = null,
    int? waitTimeSeconds = null, int? visibilityTimeout = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(queueName);

    var queueUrl = await GetQueueUrl(queueName);
    var receiveMessageRequest = new ReceiveMessageRequest
    {
      QueueUrl = queueUrl,
      AttributeNames = attributeNames ?? new List<string> { "All" },
      MessageAttributeNames = messageAttributeNames ?? new List<string> { "All" }
    };

    if (maxNumberOfMessages.HasValue) receiveMessageRequest.MaxNumberOfMessages = maxNumberOfMessages.Value;
    if (waitTimeSeconds.HasValue) receiveMessageRequest.WaitTimeSeconds = waitTimeSeconds.Value;
    if (visibilityTimeout.HasValue) receiveMessageRequest.VisibilityTimeout = visibilityTimeout.Value;

    var response = await _amazonSqsClient.ReceiveMessageAsync(receiveMessageRequest, cancellationToken);

    foreach (var message in response.Messages)
    {
      var correlationId = default(Guid?);
      if (message.MessageAttributes.TryGetValue(ContractAttributes.CorrelationId, out var correlationAttribute))
        correlationId = Guid.Parse(correlationAttribute.StringValue);

      if (!message.MessageAttributes.TryGetValue(ContractAttributes.MessageType, out var messageTypeAttribute))
      {
        await _logger.LogWarning($"Missing MessageType attribute for messageId: {message.MessageId}");
        continue;
      }

      var messageType = FindTypeFromAssembly(messageTypeAttribute.StringValue);

      if (messageType is null)
      {
        await _logger.LogWarning(
          $"Unknow MessageType: {messageTypeAttribute.StringValue} for messageId: {message.MessageId}");
        continue;
      }

      if (!messageType.GetInterfaces().Contains(typeof(IContract)))
      {
        await _logger.LogWarning(
          $"Unknow Contract Type for MessageType: {nameof(messageType)} and messageId: {message.MessageId}");
        continue;
      }

      var contract = (IContract)JsonSerializer.Deserialize(message.Body, messageType)!;

      await _logger.LogInformation(
        $"TEvent:{messageType}:queue:{queueName}:messageId:{message.MessageId}",
        requestBody: JsonSerializer.Serialize(message),
        responseBody: JsonSerializer.Serialize(contract),
        httpStatusCode: response.HttpStatusCode,
        correlationId: correlationId ?? contract.CorrelationId);

      await _mediator.Send(contract, cancellationToken);

      await _amazonSqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle,
        cancellationToken);
    }

    return response;
  }

  private static Type? FindTypeFromAssembly(string name)
  {
    return AppDomain.CurrentDomain.GetAssemblies()
      .SelectMany(r => r.GetTypes())
      .FirstOrDefault(t => t.Name == name);
  }
}