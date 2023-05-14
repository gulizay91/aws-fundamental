using Amazon.SQS.Model;

namespace User.Api.Services;

public interface IAmazonSqsService
{
  Task<string> GetQueueUrl(string queueName);

  Task<SendMessageResponse?> PublishStandartMessageAsync<TEvent>(TEvent message, string queueName,
    Dictionary<string, MessageAttributeValue>? messageAttributes = null, int? delaySeconds = null);
}