using Amazon.SQS;

namespace SqsPublisher.Clients;

public interface IAmazonClientFactory
{
  public IAmazonSQS GetSqsClient();
  public string GetSqsQueue(string queueName);
}