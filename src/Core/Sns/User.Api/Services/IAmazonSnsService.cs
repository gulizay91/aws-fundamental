using Amazon.SimpleNotificationService.Model;

namespace User.Api.Services;

public interface IAmazonSnsService
{
  ValueTask<string> GetTopicArn(string queueName);

  Task<PublishResponse?> PublishStandartMessageAsync<TEvent>(TEvent message, string queueName,
    Dictionary<string, MessageAttributeValue>? messageAttributes = null);
}