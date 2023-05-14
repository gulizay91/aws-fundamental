using Amazon.SQS.Model;

namespace User.Consumer.Services.AmazonSqs;

public interface IAmazonSqsService
{
  Task<string> GetQueueUrl(string queueName);

  Task<ReceiveMessageResponse?> SubscribeStandartMessageAsync(CancellationToken cancellationToken, string queueName,
    List<string>? attributeNames = null, List<string>? messageAttributeNames = null, int? maxNumberOfMessages = null,
    int? waitTimeSeconds = null, int? visibilityTimeout = null);
}