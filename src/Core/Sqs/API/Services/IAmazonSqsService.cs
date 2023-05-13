using Amazon.SQS.Model;

namespace API.Services;

public interface IAmazonSqsService
{
  Task<string> GetQueueUrl(string queueName);

  Task<SendMessageResponse?> PublishStandartMessageAsync<T>(T message, string queueName,
    Dictionary<string, MessageAttributeValue>? messageAttributes = null, int? delaySeconds = null);
}