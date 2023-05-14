using Amazon.SQS;

namespace User.Api.Clients;

public interface IAmazonClientFactory
{
  public IAmazonSQS GetSqsClient();
  public string GetSqsQueue(string queueName);
}