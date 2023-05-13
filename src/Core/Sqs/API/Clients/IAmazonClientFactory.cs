using Amazon.SQS;

namespace API.Clients;

public interface IAmazonClientFactory
{
  public IAmazonSQS GetSqsClient();
  public string GetSqsQueue(string queueName);
}